using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrashReport.Domain;
using CrashReport.Infrastructure.Repositories;
using NHibernate.Linq;

namespace CrashReport.Infrastructure.Nhibernate
{
	public class RepositoryBase<T>: IRepository<T> where T: DomainEntity
	{
		protected readonly ISessionProvider _sessionProvider;

		public RepositoryBase(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		IEnumerable IRepository.List()
		{
			return _sessionProvider.Session
				.Query<T>()
				.ToList();
		}

		public T GetById(int id)
		{
			return _sessionProvider.Session.Get<T>(id);
		}

		public IEnumerable<T> GetByIdsCollection(IEnumerable<int> idCollection)
		{
			var list = idCollection.ToArray();

			return _sessionProvider.Session
				.Query<T>()
				.Where(x => list.Contains(x.Id)).ToList();
		}

		public void Save(T entity)
		{
			Save((object)entity);
		}

		public void Delete(T entity)
		{
			Delete((object)entity);
		}

		void Delete(int id)
		{
			var entity = _sessionProvider.Session.Get<T>(id);
			if(entity == null) return;
			_sessionProvider.Session.Delete(entity);
			_sessionProvider.Session.Flush();
		}

		IEnumerable<T> IRepository<T>.List()
		{
			return _sessionProvider.Session
				.Query<T>()
				.ToList();
		}

		object IRepository.GetById(int id)
		{
			return GetById(id);
		}

		IEnumerable IRepository.GetByIdsCollection(IEnumerable<int> idCollection)
		{
			return GetByIdsCollection(idCollection);
		}

		public void Save(object entity)
		{
			_sessionProvider.Session.SaveOrUpdate(entity); 
			_sessionProvider.Session.Flush();
		}

		public void Delete(object entity)
		{
			_sessionProvider.Session.Delete(entity);
			_sessionProvider.Session.Flush();
		}

		void IRepository.Delete(int id)
		{
			Delete(id);
		}
	}
}