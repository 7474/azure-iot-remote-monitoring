﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GlobalResources;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Helpers;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Navigation;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Helpers
{
    /// <summary>
    /// A collection of helper methods, related to navigation.
    /// </summary>
    public static class NavigationHelper
    {
        /// <summary>
        /// Applies navigation menu item selection using the current action name.
        /// </summary>
        /// <param name="menuItems">
        /// The NavigationMenuItems, on which to apply selection.
        /// </param>
        /// <param name="controllerName">
        /// The name of the action's controller.
        /// </param>
        /// <param name="actionName">
        /// The name of the current action.
        /// </param>
        public static void ApplySelection(
            IEnumerable<NavigationMenuItem> menuItems,
            string controllerName, 
            string actionName)
        {
            if (menuItems == null)
            {
                throw new ArgumentNullException("menuItems");
            }

            var getIsSubmenuItem = ProduceGetIsSubnav();

            menuItems = menuItems.Where(t => (t != null) && (t.Controller == controllerName));
            foreach (var navigationMenuItem in menuItems)
            {
                if ( navigationMenuItem.Action == actionName || getIsSubmenuItem(controllerName, actionName))
                {
                    navigationMenuItem.Selected = true;
                    navigationMenuItem.Class = string.Format(CultureInfo.CurrentCulture, "{0} {1}", navigationMenuItem.Class, "selected");
                }
            }
        }

        /// <summary>
        /// Applies submenu navigation menu item selection using the current action name.
        /// </summary>
        /// <param name="menuItems">
        /// The NavigationMenuItems, on which to apply selection.
        /// </param>
        /// <param name="controllerName">
        /// The name of the action's controller.
        /// </param>
        /// <param name="actionName">
        /// The name of the current action.
        /// </param>
        public static void ApplySubmenuSelection(
            IEnumerable<NavigationMenuItem> menuItems,
            string controllerName,
            string actionName)
        {
            if (menuItems == null)
            {
                throw new ArgumentNullException("menuItems");
            }

            menuItems = menuItems.Where(t => t != null);
            foreach (var navigationMenuItem in menuItems)
            {
                if (navigationMenuItem.Controller == controllerName && navigationMenuItem.Action == actionName)
                {
                    navigationMenuItem.Selected = true;
                    navigationMenuItem.Class = string.Format(CultureInfo.CurrentCulture, "{0} {1}", navigationMenuItem.Class, "selected");
                }
            }
        }

        /// <summary>
        /// Gets the NavigationMenuItem that describe a controller's 
        /// subnavigation items.
        /// </summary>
        /// <param name="controllerName">
        /// The name of the controller.
        /// </param>
        /// <returns>
        /// NavigationMenuItems that describe a controller's subnavigation items.
        /// </returns>
        public static List<NavigationMenuItem> GetSubnavigationItemsForController(
            string controllerName)
        {
            if (controllerName == "Advanced")
            {
                return GetAdvancedControllerSubmenuItems();
            }

            return new List<NavigationMenuItem>();
        }

        private static List<NavigationMenuItem> GetAdvancedControllerSubmenuItems()
        {
            var items = new List<NavigationMenuItem>();

            items.Add(new NavigationMenuItem()
            {
                Action = "HealthBeat",
                Controller ="Advanced",
                Text = Strings.HealthBeat
            });

            items.Add(new NavigationMenuItem()
            {
                Action = "LogicApps",
                Controller = "Advanced",
                Text = Strings.LogicApps
            });

            items.Add(new NavigationMenuItem()
            {
                Action = "CellularConn",
                Controller = "Advanced",
                Text = Strings.CellularConn
            });

            return items;
        }

        private static Func<string, string, bool> ProduceGetIsSubnav()
        {
            var getControllerSubnavs =
                FunctionalHelper.Memoize<string, HashSet<string>>(
                    (controllerName) =>
                    {
                        IEnumerable<NavigationMenuItem> navItems = GetSubnavigationItemsForController(controllerName);

                        if (navItems == null)
                        {
                            navItems = new NavigationMenuItem[0];
                        }

                        navItems = navItems.Where(
                            t =>
                                (t != null) &&
                                !string.IsNullOrEmpty(t.Action));

                        return new HashSet<string>(navItems.Select(t => t.Action));
                    });

            return (controllerName, actionName) =>
            {
                if (string.IsNullOrEmpty(actionName))
                {
                    return false;
                }

                return getControllerSubnavs(controllerName).Contains(actionName);
            };
        }
    }
}