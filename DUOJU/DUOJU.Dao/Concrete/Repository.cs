using DUOJU.Domain.Entities;

namespace DUOJU.Dao.Concrete
{
    public abstract class Repository
    {
        private readonly object padlock = new object();
        private static DUOJUEntities _dbEntities { get; set; }

        public DUOJUEntities DBEntities
        {
            get
            {
                if (_dbEntities == null)
                {
                    lock (padlock)
                    {
                        if (_dbEntities == null)
                            _dbEntities = new DUOJUEntities();
                    }
                }

                return _dbEntities;
            }
        }
    }
}
