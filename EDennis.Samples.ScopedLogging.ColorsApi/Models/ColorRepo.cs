using EDennis.AspNetCore.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Models {
    public class ColorRepo {
        private readonly ColorDbContext context;
        private readonly ILogger logger;
        private readonly ScopeProperties scopeProperties;

        public ColorRepo(ColorDbContext context, ScopeProperties scopeProperties, ILogger<ColorRepo> logger) {
            this.context = context;
            this.scopeProperties = scopeProperties;
            this.logger = logger;
        }

        public virtual IEnumerable<Color> GetColors() {
            var colors = context.Color.ToList();
            return colors;
        }

        public virtual async Task<Color> GetColor(int id) {

            var color = await context.Color.FindAsync(id);
            return color;
        }


        public virtual async Task UpdateColor(Color color) {

            context.Entry(color).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return;
        }

        public virtual async Task<Color> CreateColor(Color color) {
            context.Color.Add(color);
            await context.SaveChangesAsync();

            return color;
        }

        public virtual async Task<Color> DeleteColor(int id) {
            var color = await context.Color.FindAsync(id);

            if (color == null) {
                return null;
            }

            context.Color.Remove(color);
            await context.SaveChangesAsync();

            return color;
        }

        public virtual bool ColorExists(int id) {
            return context.Color.Any(e => e.Id == id);
        }


    }
}