using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Linq;
using DeliveryTracker.Models;
using UpshotHelper.Controllers;
using UpshotHelper.Models;

namespace DeliveryTracker.Controllers
{
    public class DataServiceController : UpshotController 
    {
        private AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataServiceController" /> class.
        /// </summary>
        public DataServiceController()
        {
            _dbContext = new AppDbContext();
        }

        /// <summary>
        /// Gets the deliveries for today.
        /// </summary>
        /// <returns>Returns the Deliveries for today.</returns>
        public IQueryable<Delivery> GetDeliveriesForToday()
        {
            // Could pre-filter by due date, delivery driver, etc...
            return _dbContext.Deliveries.Include("Customer").OrderBy(x => x.DeliveryId);
        }

        /// <summary>
        /// Processes the submit.
        /// </summary>
        /// <param name="changeSet">The change set.</param>
        /// <returns>Returns True if successful, otherwise false.</returns>
        protected override bool ProcessSubmit(ChangeSet changeSet)
        {
            bool success = true;

			try
			{
				foreach (ChangeSetEntry entry in changeSet.ChangeSetEntries)
				{
                    switch (entry.Operation)
                    {
                        case ChangeOperation.Update:
                            DbEntityEntry deliveryEntry = _dbContext.Entry(entry.Entity);

                            deliveryEntry.State = EntityState.Modified;
                            _dbContext.SaveChanges();
                            break;
                    }
				}

			}
			catch (Exception ex)
			{
				success = false;
			}

			return success;
        }
    }
}
