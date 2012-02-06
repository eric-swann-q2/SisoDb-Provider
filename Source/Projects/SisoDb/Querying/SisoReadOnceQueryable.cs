using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using SisoDb.Resources;

namespace SisoDb.Querying
{
	public class SisoReadOnceQueryable<T> : SisoQueryable<T> where T : class 
	{
		private readonly Func<ISession> _queryEngineFactory;

		protected override ISession Session
		{
			get { return _queryEngineFactory.Invoke(); }
		}

		public SisoReadOnceQueryable(IQueryBuilder<T> queryBuilder, Func<ISession> queryEngineFactory) : base(queryBuilder)
		{
			Ensure.That(queryEngineFactory, "queryEngineFactory").IsNotNull();

			_queryEngineFactory = queryEngineFactory;
		}

		public override int Count()
		{
			using (var session = Session)
			{
				return session.QueryEngine.Count<T>(QueryBuilder.Build());
			}
		}

		public override int Count(System.Linq.Expressions.Expression<Func<T, bool>> expression)
		{
			using (var session = Session)
			{
				return session.QueryEngine.Count<T>(QueryBuilder.Build());
			}
		}
		
		public override T First()
		{
			using (var session = Session)
			{
				return session.QueryEngine.Query<T>(QueryBuilder.Build()).First();
			}
		}

		public override TResult FirstAs<TResult>()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAs<T, TResult>(QueryBuilder.Build()).First();
			}
		}

		public override string FirstAsJson()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAsJson<T>(QueryBuilder.Build()).First();
			}
		}

		public override T FirstOrDefault()
		{
			using (var session = Session)
			{
				return session.QueryEngine.Query<T>(QueryBuilder.Build()).FirstOrDefault();
			}
		}

		public override TResult FirstOrDefaultAs<TResult>()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAs<T, TResult>(QueryBuilder.Build()).FirstOrDefault();
			}
		}

		public override string FirstOrDefaultAsJson()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAsJson<T>(QueryBuilder.Build()).FirstOrDefault();
			}
		}

		public override T[] ToArray()
		{
			using (var session = Session)
			{
				return session.QueryEngine.Query<T>(QueryBuilder.Build()).ToArray();
			}
		}

		public override TResult[] ToArrayOf<TResult>()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAs<T, TResult>(QueryBuilder.Build()).ToArray();
			}
		}

        public override TResult[] ToArrayOf<TResult>(TResult template)
        {
            using (var session = Session)
            {
                return session.QueryEngine.QueryAsAnonymous<T, TResult>(QueryBuilder.Build(), template).ToArray();
            }
        }

		public override string[] ToArrayOfJson()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAsJson<T>(QueryBuilder.Build()).ToArray();
			}
		}

		public override IEnumerable<T> ToEnumerable()
		{
			throw new SisoDbException(ExceptionMessages.ReadOnceQueryable_YieldingNotSupported);
		}

		public override IEnumerable<TResult> ToEnumerableOf<TResult>()
		{
			throw new SisoDbException(ExceptionMessages.ReadOnceQueryable_YieldingNotSupported);
		}

	    public override IEnumerable<TResult> ToEnumerableOf<TResult>(TResult template)
	    {
	        throw new SisoDbException(ExceptionMessages.ReadOnceQueryable_YieldingNotSupported);
	    }

	    public override IEnumerable<string> ToEnumerableOfJson()
		{
			throw new SisoDbException(ExceptionMessages.ReadOnceQueryable_YieldingNotSupported);
		}

	    public override IList<T> ToList()
		{
			using (var session = Session)
			{
				return session.QueryEngine.Query<T>(QueryBuilder.Build()).ToList();
			}
		}

	    public override IList<TResult> ToListOf<TResult>()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAs<T, TResult>(QueryBuilder.Build()).ToList();
			}
		}

        public override IList<TResult> ToListOf<TResult>(TResult template)
        {
            using (var session = Session)
            {
                return session.QueryEngine.QueryAsAnonymous<T, TResult>(QueryBuilder.Build(), template).ToList();
            }
        }

	    public override IList<string> ToListOfJson()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAsJson<T>(QueryBuilder.Build()).ToList();
			}
		}

	    public override T Single()
		{
			using (var session = Session)
			{
				return session.QueryEngine.Query<T>(QueryBuilder.Build()).Single();
			}
		}

		public override TResult SingleAs<TResult>()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAs<T, TResult>(QueryBuilder.Build()).Single();
			}
		}

		public override string SingleAsJson()
		{
			using (var session =Session)
			{
				return session.QueryEngine.QueryAsJson<T>(QueryBuilder.Build()).Single();
			}
		}

		public override T SingleOrDefault()
		{
			using (var session = Session)
			{
				return session.QueryEngine.Query<T>(QueryBuilder.Build()).SingleOrDefault();
			}
		}

		public override TResult SingleOrDefaultAs<TResult>()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAs<T, TResult>(QueryBuilder.Build()).SingleOrDefault();
			}
		}

		public override string SingleOrDefaultAsJson()
		{
			using (var session = Session)
			{
				return session.QueryEngine.QueryAsJson<T>(QueryBuilder.Build()).SingleOrDefault();
			}
		}
	}
}