using System;
using System.Linq;

using PetStore.Data;
using PetStore.Data.Models;

namespace PetStore.Services
{
    public class MerchandiseTypeService : IMerchandiseTypeService
    {
        private PetStoreDbContext context;

        public MerchandiseTypeService(PetStoreDbContext context)
        {
            this.context = context;
        }

        public void CreateMerchandiseType(string name)
        {
            if (context.MerchandiseTypes.Any(x => x.Name == name))
            {
                return;
            }

            var merchandise = new MerchandiseType() {Name = name};

            context.MerchandiseTypes.Add(merchandise);
            context.SaveChanges();
        }

        public bool RemoveMerchandiseType(string name)
        {
            var merchanandise = context.MerchandiseTypes.FirstOrDefault(mt => mt.Name == name);

            if (merchanandise == null)
            {
                return false;
            }
            else
            {
                try
                {
                    context.MerchandiseTypes.Remove(merchanandise);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return true;
            }
        }

        public bool RemoveMerchandiseType(int id)
        {
            var merchanandise = context.MerchandiseTypes.FirstOrDefault(mt => mt.Id == id);

            if (merchanandise == null)
            {
                return false;
            }
            else
            {
                try
                {
                    context.MerchandiseTypes.Remove(merchanandise);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return true;
            }
        }
    }
}
