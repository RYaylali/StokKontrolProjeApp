using Microsoft.EntityFrameworkCore;
using StokKontrol.Domain.Entities;
using StokKontrol.Repository.Abstarct;
using StokKontrol.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StokKontrol.Repository.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StokKontrolContext _context;
        public GenericRepository(StokKontrolContext context)
        {
            _context = context;
        }
        

        public bool Add(T item)
        {
            try
            {
                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item);
                return Save() > 0;//sadece 1 satır döndürüyorsa true döndürür.(etkilenen ya da değişen satır sayısı)
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                //Ram üzerinde kullanılmayan dosyanın belli süre geçtikten sonra Ram'den temizleyen blok-->Garbag blok
                //using için amacı garbag blok'u beklememek
                using (TransactionScope ts = new TransactionScope())
                {
                    //Veri tabanında rol beklediğimiz olay herhangi bir işlem adımında sorun yaşarsan en başa dön gibi işlem yapar. işlem hatasına kadar yapılan adımlar kaydedilir. bunun en başa dönmesini sağlar. buna da rollback denir.-->TransactionScope
                    // _context.Set<T>().AddRange(items);

                    foreach (T item in items)
                    {
                        item.AddedDate = DateTime.Now;
                        _context.Set<T>().Add(item);
                    }
                    ts.Complete();
                    return Save() > 0;//1 veya daha fazla satır eklneiyorsa true döndürür....
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool Any(Expression<Func<T, bool>> exp) => _context.Set<T>().Any();//Herhangi bir şey var mı yok mu ona bakacak

        public List<T> GetActive() => _context.Set<T>().Where(x => x.IsActive == true).ToList();



        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.IsActive == true);

            if (includes != null)
            {
                query = includes.Aggregate(query,(current,include)=>current.Include(include));//current o anki query den dönen tablom, include ise onunla ilişkili olacak olan tablom
            }
            return query;
        }

        public List<T> GetAll() => _context.Set<T>().ToList();
        

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));//current o anki query den dönen tablom, include ise onunla ilişkili olacak olan tablom
            }
            return query;
        }


        public T GetByDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().FirstOrDefault(exp);



        public T GetById(int id)=>_context.Set<T>().Find(id);

        

        public IQueryable<T> GetByID(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x=>x.Id==id);

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));//current o anki query den dönen tablom, include ise onunla ilişkili olacak olan tablom
            }
            return query;
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)=>_context.Set<T>().Where(exp).ToList();
       

        public bool Remove(T item)
        {
            item.IsActive = false;
            return Update(item);
        }

        public bool Remove(int id)
        {
            try
            {
                using (TransactionScope ts=new TransactionScope())
                {
                    T item = GetById(id);
                    item.IsActive = false;
                    ts.Complete();
                    return Update(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var collection = GetDefault(exp);//Verilen ifadeye göre ilgili nesneleri collection'a atıyoruz
                    var counter = 0;
                    foreach (var item in collection)
                    {
                        item.IsActive = false;
                        bool operationResult=Update(item);
                        if (operationResult) counter++;//sıradaki item'in silinme ,şlemi(yani silindi işaretleme) başarılı ise sayacı 1 arttırıyoruz.
                    }
                    if (collection.Count == counter) ts.Complete();//Koleksiyondaki eleman sayısı ile silme işleminde gerçekleşen eleman sayısıcounter'daki sayı) eşit ise bu işlemlerin tümü başarılırıdr.
                    else return false;

                    
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool Activate(int id)
        {
            T item = GetById(id);
            item.IsActive = true;
            return Update(item);
        }

        public int Save()
        {
            return _context.SaveChanges();//db'e kaydedilenlerin sayısını döner.(SQL de kategori tablosuna kategoriler ekledik kaç tane ekledik savechanges onu döner.)
        }

        public bool Update(T item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false; ;
            }
        }

        public void DetachEntity(T item)
        {
            _context.Entry<T>(item).State=EntityState.Detached;//bir entry'i takip etmeyi bırakmak için kullanılır.
        }

    }
}
