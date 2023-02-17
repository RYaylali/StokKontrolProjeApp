using StokKontrol.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol.Repository.Abstarct
{
    public interface IGenericRepository<T> where T: BaseEntity//T yerine başka bir ifade de kullanılabilir. User mı Product mı belli olmaddığı için where T: Class de diyebilirz burada BaseEntityden inherit olmuş classlar olsun diyoruz.
    {

        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T,bool>>exp);
        T GetById(int id);//idd'yeöre nesne getir dedik hepsi için repository oluşturmak yerine hangi nesne için çalışıyorsak T yerine ogelsin dedik
        IQueryable<T> GetByID(int id, params Expression<Func<T, object>>[] includes);//bağlıolduğu verileri de getir
        T GetByDefault(Expression<Func<T, bool>> exp);
        List<T> GetActive();
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes);
        List<T> GetDefault(Expression<Func<T, bool>> exp);//Listelerini döndürecek
        List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        bool Activate(int id);
        bool Any(Expression<Func<T, bool>> exp);
        int Save();
        void DetachEntity(T item);
    }
}
