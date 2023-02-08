using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {
        private readonly HashSet<Route> routes;
        private readonly IDictionary<string, Route> routesById;

        public MoovIt()
        {
            this.routes = new HashSet<Route>();
            this.routesById = new Dictionary<string, Route>();
        }

        public int Count => this.routes.Count;

        public void AddRoute(Route route)
        {
            if (this.routes.Contains(route))
            {
                throw new ArgumentException();
            }

            this.routesById.Add(route.Id, route);
            this.routes.Add(route);
        }

        public void ChooseRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            this.routesById[routeId].Popularity++;
        }

        public bool Contains(Route route)
        {
            return this.routes.Contains(route);
        }

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint)
        {
            return this.routes.Where(r => r.IsFavorite
                && r.LocationPoints.Contains(destinationPoint)
                && r.LocationPoints[0] != destinationPoint)
                .OrderBy(r => r.Distance)
                .ThenByDescending(r => r.Popularity)
                .ToArray();
        }

        public Route GetRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            return this.routesById[routeId];
        }

        public IEnumerable<Route> GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints()
        {
            return this.routes
                .OrderByDescending(r => r.Popularity)
                .ThenBy(r => r.Distance)
                .ThenBy(r => r.LocationPoints.Count)
                .Take(5)
                .ToArray();
        }

        public void RemoveRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            Route routeToRemove = this.routesById[routeId];
            this.routes.Remove(routeToRemove);
            this.routesById.Remove(routeId);
        }

        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint)
        {
            return this.routes
                .Where(r => r.LocationPoints.Contains(startPoint)
                && r.LocationPoints.Contains(endPoint)
                && r.LocationPoints.IndexOf(startPoint) <= r.LocationPoints.IndexOf(endPoint))
                .OrderBy(r => r.IsFavorite)
                .OrderBy(r => r.LocationPoints.IndexOf(endPoint) - r.LocationPoints.IndexOf(startPoint))
                .ThenByDescending(r => r.Popularity)
                .ToArray();
        }
    }
}
