import { RouteNames } from "@/router/routes";
import {
  Search,
  Building2,
  Settings,
  User,
  LogOut,
  CreditCard,
  LogIn,
  UserRoundPlus,
} from "lucide-react";
import { logout } from "./auth";

type NavigationItem = {
  title: string;
  icon: React.ComponentType<React.SVGProps<SVGSVGElement>>;
  url: string;
  permission?: string;
};

type MenuItemLink = {
  title: string;
  icon: React.ComponentType<React.SVGProps<SVGSVGElement>>;
  url: string;
  razor?: boolean;
};

type MenuItemAction = {
  title: string;
  icon: React.ComponentType<React.SVGProps<SVGSVGElement>>;
  action: () => void;
};

export type MenuItem = MenuItemLink | MenuItemAction;

export const navigationItems: NavigationItem[] = [
  {
    title: "Search Accommodations",
    icon: Search,
    url: RouteNames.HOME,
  },
  {
    title: "My Properties",
    icon: Building2,
    url: RouteNames.CREATE_PROPERTY,
    permission: "create:property",
  },
];

export const AUTHENTICATED_MENU_ITEMS: MenuItem[] = [
  {
    title: "My Profile",
    icon: User,
    url: RouteNames.HOME,
  },
  {
    title: "Billing",
    icon: CreditCard,
    url: RouteNames.HOME,
  },
  {
    title: "Settings",
    icon: Settings,
    url: RouteNames.HOME,
  },
  {
    title: "Log Out",
    icon: LogOut,
    action() {
      logout();
    },
  },
];

export const NO_AUTHENTICATED_ITEMS: MenuItem[] = [
  {
    title: "Login",
    icon: LogIn,
    url: "/Login",
    razor: true,
  },
  {
    title: "Register",
    icon: UserRoundPlus,
    url: "/Register",
    razor: true,
  },
];
