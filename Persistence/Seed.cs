using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {

            if (!context.CreditCards.Any())
            {
                var CreditCards = new List<CreditCard>(){
                    new CreditCard{Name = "Bob Smith", CardNumber= 1111222233334441, CVC=123, Expiry= DateTime.Now.AddDays(30)},
                    new CreditCard{Name = "Tom Smith", CardNumber= 1111222233334442, CVC=123, Expiry= DateTime.Now.AddDays(30)},
                    new CreditCard{Name = "Jane Smith", CardNumber= 1111222233334443, CVC=123, Expiry= DateTime.Now.AddDays(30)},
                    new CreditCard{Name = "David Smith", CardNumber= 1111222233334444, CVC=123, Expiry= DateTime.Now.AddDays(30)},
                };

                context.CreditCards.AddRange(CreditCards);
                await context.SaveChangesAsync();
            }
        }
    }
}