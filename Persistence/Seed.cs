using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
    public static class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (context.TodoItems.Any()) return;

            var todoItems = new List<TodoItem> {
                new TodoItem {
                    Name = "Test"
                }
            };

            await context.TodoItems.AddRangeAsync(todoItems);
            await context.SaveChangesAsync();
        }
    }
}