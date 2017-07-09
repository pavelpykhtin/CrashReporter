using System.Collections;
using System.Collections.Generic;

namespace CrashReport.Infrastructure.Repositories
{
	public interface IRepository
	{
		IEnumerable List();
		object GetById(int id);
		IEnumerable GetByIdsCollection(IEnumerable<int> idCollection);

		void Save(object entity);
		void Delete(int id);
	}

	public interface IRepository<T>: IRepository
	{
		new IEnumerable<T> List();
		new T GetById(int id);
	
		void Save(T entity);

		void Delete(T entity);
		new IEnumerable<T> GetByIdsCollection(IEnumerable<int> idCollection);
	}
}