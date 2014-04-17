/**
* @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
* For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.skin = 'bootstrapck';

    config.toolbar = 'MyToolbar';

    config.toolbar_MyToolbar =
    [
        ['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print', 'SpellChecker', 'Scayt'],
        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
        ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
        ['Link', 'Unlink', 'Anchor'],
        ['Maximize'],
        '/',
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor']
    ];


    config.contentsCss = ['http://cdn.yaochufa.com/style/reset.css?v=6.0', 'http://www.yaochufa.com/style/other.css?v=8.0', 'http://www.yaochufa.com/style/tuanview.css?v=1', '/ckeditor/contents.css'];

    config.allowedContent = true;
    config.removeDialogTabs = 'image:advanced;image:Link;';
    config.image_previewText = CKEDITOR.tools.repeat(' ', 1); ;

    config.filebrowserImageUploadUrl = '/Image/UploadImage?imageType=ProductContent';
};
