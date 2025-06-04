"use client";
import { Building2, Bell, ChevronLeft } from "lucide-react";
import { useMemo } from "react";
import {
  Link,
  matchPath,
  Outlet,
  useLocation,
  useMatches,
} from "react-router-dom";

import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarProvider,
  SidebarInset,
  SidebarRail,
} from "@/components/ui/sidebar";

import { useGetUserSummary } from "@/queries/users.queries";
import { useAuthStore } from "@/stores/auth.store";
import {
  AUTHENTICATED_MENU_ITEMS,
  navigationItems,
  NO_AUTHENTICATED_ITEMS,
  type MenuItem,
} from "@/utils/layout";
import type { RouteConfig } from "@/router";

import CanAccess from "./auth/CanAccess";

const RenderMenuItem = (item: MenuItem) => {
  if ("action" in item) {
    return (
      <DropdownMenuItem>
        <button
          className="w-full flex items-center gap-2 cursor-pointer"
          onClick={item.action}
        >
          <item.icon className="mr-2 h-4 w-4" />
          <span>{item.title}</span>
        </button>
      </DropdownMenuItem>
    );
  }

  return (
    <DropdownMenuItem>
      {item.razor ? (
        <a href={item.url} className="w-full flex items-center gap-2">
          <item.icon className="mr-2 h-4 w-4" />
          <span>{item.title}</span>
        </a>
      ) : (
        <Link to={item.url} className="w-full flex items-center gap-2">
          <item.icon className="mr-2 h-4 w-4" />
          <span>{item.title}</span>
        </Link>
      )}
    </DropdownMenuItem>
  );
};

export default function SkyBnBLayout() {
  const { pathname } = useLocation();
  const matches = useMatches();

  const { isAuthenticated } = useAuthStore();
  const { user, isUserLoading, isUserError } =
    useGetUserSummary(isAuthenticated);

  const currentRoute = matches.at(-1);
  const { layoutConfig = {} } = currentRoute?.handle as RouteConfig;

  const userInitials = useMemo(() => {
    if (!user) return "NN";
    const firstNameInitial = user.firstName?.charAt(0).toUpperCase() || "";
    const lastNameInitial = user.lastName?.charAt(0).toUpperCase() || "";

    return `${firstNameInitial}${lastNameInitial}`;
  }, [user]);

  const menuItems = useMemo(() => {
    if (isAuthenticated) return AUTHENTICATED_MENU_ITEMS;
    return NO_AUTHENTICATED_ITEMS;
  }, [isAuthenticated]);

  return (
    <SidebarProvider>
      <Sidebar className="border-r">
        <SidebarHeader className="border-b px-6 py-4">
          <div className="flex items-center gap-2">
            <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-primary text-primary-foreground">
              <Building2 className="h-4 w-4" />
            </div>
            <div className="flex flex-col">
              <span className="text-lg font-bold text-primary">SkyBnB</span>
              <span className="text-xs text-muted-foreground">
                Your accommodation platform
              </span>
            </div>
          </div>
        </SidebarHeader>

        <SidebarContent>
          <SidebarGroup>
            <SidebarGroupContent>
              <SidebarMenu>
                {navigationItems.map((item) => (
                  <CanAccess key={item.title} permission={item.permission}>
                    <SidebarMenuItem key={item.title}>
                      <SidebarMenuButton
                        asChild
                        isActive={matchPath(item.url, pathname) !== null}
                      >
                        <Link to={item.url} className="flex items-center gap-3">
                          <item.icon className="h-4 w-4" />
                          <span>{item.title}</span>
                        </Link>
                      </SidebarMenuButton>
                    </SidebarMenuItem>
                  </CanAccess>
                ))}
              </SidebarMenu>
            </SidebarGroupContent>
          </SidebarGroup>
        </SidebarContent>
        <SidebarRail />
      </Sidebar>

      <SidebarInset>
        {/* Header con menú de usuario */}
        <header className="sticky top-0 z-10 flex h-16 bg-background shrink-0 items-center justify-between border-b px-6">
          <div className="flex items-center gap-2">
            {layoutConfig.backTo && (
              <Link to={layoutConfig.backTo}>
                <ChevronLeft className="h-4 w-4" />
              </Link>
            )}
            <h1 className="text-xl font-semibold">
              {layoutConfig.title ?? "SkyBnB"}
            </h1>
          </div>

          <div className="flex items-center gap-4">
            {/* Notificaciones */}
            <Button variant="ghost" size="icon">
              <Bell className="h-4 w-4" />
            </Button>

            {/* Menú de usuario */}
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button
                  variant="ghost"
                  className="relative h-8 w-8 rounded-full"
                >
                  {" "}
                  <Avatar className="h-8 w-8">
                    <AvatarImage src="/placeholder-user.jpg" alt="User" />
                    {userInitials && (
                      <AvatarFallback>{userInitials}</AvatarFallback>
                    )}
                  </Avatar>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className="w-56" align="end" forceMount>
                <DropdownMenuLabel className="font-normal">
                  {isAuthenticated && (
                    <div className="flex flex-col space-y-1">
                      <p className="text-sm font-medium leading-none">
                        {isUserLoading ? (
                          "Loading..."
                        ) : isUserError ? (
                          "Error loading name"
                        ) : (
                          <>
                            <span className="capitalize">
                              {user?.firstName ?? "No"}
                            </span>{" "}
                            <span className="capitalize">
                              {user?.lastName ?? "User"}
                            </span>
                          </>
                        )}
                      </p>
                      <p className="text-xs leading-none text-muted-foreground">
                        {isUserLoading
                          ? "Loading..."
                          : isUserError
                          ? "Error loading email"
                          : user?.email ?? ""}
                      </p>
                    </div>
                  )}
                  {!isAuthenticated && (
                    <p className="text-xs leading-none text-muted-foreground">
                      Not logged in
                    </p>
                  )}
                </DropdownMenuLabel>
                <DropdownMenuSeparator />{" "}
                {menuItems.map((item) => (
                  <RenderMenuItem key={item.title} {...item} />
                ))}
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </header>

        {/* Contenido principal */}
        <section className="flex-1 space-y-4 p-6">
          <Outlet />
        </section>
      </SidebarInset>
    </SidebarProvider>
  );
}
