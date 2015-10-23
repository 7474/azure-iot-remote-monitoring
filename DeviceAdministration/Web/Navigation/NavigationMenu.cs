﻿using System.Collections.Generic;
using GlobalResources;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Security;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Navigation
{
    public class NavigationMenu
    {
        private readonly List<NavigationMenuItem> _navigationMenuItems;

        public NavigationMenu()
        {
            _navigationMenuItems = new List<NavigationMenuItem>()
            {
                new NavigationMenuItem
                {
                    Text = Strings.NavigationMenuItemDashboard,
                    Action = "Index",
                    Controller = "Dashboard",
                    Selected = false,
                    Class = "nav_dashboard",
                    MinimumPermission = Permission.ViewTelemetry,
                },
                new NavigationMenuItem
                {
                    Text = Strings.NavigationMenuItemDevices,
                    Action = "Index",
                    Controller = "Device",
                    Selected = false,
                    Class = "nav_devices",
                    MinimumPermission = Permission.ViewDevices,
                },
                new NavigationMenuItem
                {
                    Text = Strings.Rules,
                    Action = "Index",
                    Controller = "DeviceRules",
                    Selected = false,
                    Class = "nav_view_rules",
                    MinimumPermission = Permission.ViewRules,
                },
                new NavigationMenuItem
                {
                    Text = Strings.NavigationMenuItemActions,
                    Action = "Index",
                    Controller = "Actions",
                    Selected = false,
                    Class = "nav_actions",
                    MinimumPermission = Permission.ViewActions,
                },
                new NavigationMenuItem
                {
                    Text = Strings.NavigationMenuItemsAdvanced,
                    Action = "HealthBeat",
                    Controller = "Advanced",
                    Selected = false,
                    Class = "nav_advanced",
                    MinimumPermission = Permission.ViewAdvanced,
                },
            };
        }

        public List<NavigationMenuItem> NavigationMenuItems
        {
            get
            {
                // only show menu items the user has permission for
                var visibleItems = new List<NavigationMenuItem>();
                foreach(var menuItem in _navigationMenuItems)
                {
                    if (PermsChecker.HasPermission(menuItem.MinimumPermission))
                    {
                        visibleItems.Add(menuItem);
                    }
                }

                return visibleItems;
            }
        }
    }
}