using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DUOJU.WECHAT.Sys.Attributes;
using DUOJU.WECHAT.Sys.Helpers;
using System.Linq;

namespace DUOJU.WECHAT.App_Start
{
    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public CustomModelMetadataProvider(Type defaultResourceType, bool requireConventionAttribute)
        {
            DefaultResourceType = defaultResourceType;
            RequireConventionAttribute = requireConventionAttribute;
        }

        public Type DefaultResourceType
        {
            get;
            private set;
        }

        public bool RequireConventionAttribute
        {
            get;
            private set;
        }

        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            Func<IEnumerable<Attribute>, ModelMetadata> metadataFactory = (attr) => base.CreateMetadata(attr, containerType, modelAccessor, modelType, propertyName);

            var conventionType = containerType ?? modelType;

            Type defaultResourceType = DefaultResourceType;
            MetadataConventionsAttribute conventionAttribute = conventionType.GetAttributeOnTypeOrAssembly<MetadataConventionsAttribute>();

            if (conventionAttribute != null && conventionAttribute.ResourceType != null)
            {
                defaultResourceType = conventionAttribute.ResourceType;
            }
            else if (RequireConventionAttribute)
            {
                return metadataFactory(attributes);
            }

            ApplyConventionsToValidationAttributes(attributes, containerType, propertyName, defaultResourceType);

            var foundDisplayAttribute = attributes.FirstOrDefault(a => typeof(DisplayAttribute) == a.GetType()) as DisplayAttribute;

            if (foundDisplayAttribute.CanSupplyDisplayName())
            {
                return metadataFactory(attributes);
            }

            // Our displayAttribute is lacking. Time to get busy.
            DisplayAttribute displayAttribute = foundDisplayAttribute.Copy() ?? new DisplayAttribute();

            var rewrittenAttributes = attributes.Replace(foundDisplayAttribute, displayAttribute);

            // ensure resource type.
            displayAttribute.ResourceType = displayAttribute.ResourceType ?? defaultResourceType;

            if (displayAttribute.ResourceType != null)
            {
                // ensure resource name
                string displayAttributeName = GetDisplayAttributeName(containerType, propertyName, displayAttribute);

                if (displayAttributeName != null)
                {
                    displayAttribute.Name = displayAttributeName;
                }

                if (!displayAttribute.ResourceType.PropertyExists(displayAttribute.Name))
                {
                    displayAttribute.ResourceType = null;
                }
            }

            var metadata = metadataFactory(rewrittenAttributes);

            if (metadata.DisplayName == null || metadata.DisplayName == metadata.PropertyName)
            {
                metadata.DisplayName = metadata.PropertyName.SplitUpperCaseToString();
            }

            return metadata;
        }

        private static void ApplyConventionsToValidationAttributes(IEnumerable<Attribute> attributes, Type containerType, string propertyName, Type defaultResourceType)
        {
            foreach (ValidationAttribute validationAttribute in attributes.Where(x => (x as ValidationAttribute != null)))
            {
                if (string.IsNullOrEmpty(validationAttribute.ErrorMessage))
                {
                    string attributeShortName = validationAttribute.GetType().Name.Replace("Attribute", "");

                    string[] defaultAttributes = new string[] { "Required", "StringLength" };

                    string resourceKey = defaultAttributes.Contains(attributeShortName) ? "_" + attributeShortName : GetResourceKey(containerType, propertyName) + "_" + attributeShortName;

                    var resourceType = validationAttribute.ErrorMessageResourceType ?? defaultResourceType;

                    if (!resourceType.PropertyExists(resourceKey))
                    {
                        resourceKey = "Error_" + attributeShortName;

                        if (!resourceType.PropertyExists(resourceKey))
                            continue;

                        validationAttribute.ErrorMessageResourceType = resourceType;
                        validationAttribute.ErrorMessageResourceName = resourceKey;
                    }
                    else
                    {
                        validationAttribute.ErrorMessageResourceType = resourceType;
                        validationAttribute.ErrorMessageResourceName = resourceKey;
                    }
                }
            }
        }

        private static string GetDisplayAttributeName(Type containerType, string propertyName, DisplayAttribute displayAttribute)
        {
            if (containerType != null)
            {
                if (String.IsNullOrEmpty(displayAttribute.Name))
                {
                    // check to see that resource key exists.
                    string resourceKey = GetResourceKey(containerType, propertyName);
                    if (displayAttribute.ResourceType.PropertyExists(resourceKey))
                        return resourceKey;
                    else
                        return propertyName;
                }
            }

            return null;
        }

        private static string GetResourceKey(Type containerType, string propertyName)
        {
            return containerType.Name + "_" + propertyName;
        }
    }
}