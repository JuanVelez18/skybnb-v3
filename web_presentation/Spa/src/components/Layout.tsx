"use client";
import {
  Search,
  Building2,
  Settings,
  User,
  LogOut,
  CreditCard,
  Bell,
} from "lucide-react";
import { useEffect, useMemo } from "react";
import { Link, Outlet, useLocation } from "react-router-dom";

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

import { RouteNames } from "@/router/routes";
import { initializeSession, logout } from "@/utils/auth";
import { useGetUserSummary } from "@/queries/users.queries";

const navigationItems = [
  {
    title: "Search Accommodations",
    icon: Search,
    url: RouteNames.HOME,
  },
  {
    title: "My Properties",
    icon: Building2,
    url: RouteNames.CREATE_PROPERTY,
  },
];

export default function SkyBnBLayout() {
  const { pathname } = useLocation();
  const { user, isUserLoading, isUserError } = useGetUserSummary();

  const title = useMemo(() => {
    switch (pathname) {
      case RouteNames.HOME:
        return "Search Accommodations";
      case RouteNames.CREATE_PROPERTY:
        return "Create New Property";
      default:
        return "SkyBnB";
    }
  }, [pathname]);

  useEffect(() => {
    initializeSession();
  }, []);

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
                  <SidebarMenuItem key={item.title}>
                    <SidebarMenuButton asChild>
                      <Link to={item.url} className="flex items-center gap-3">
                        <item.icon className="h-4 w-4" />
                        <span>{item.title}</span>
                      </Link>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
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
            <h1 className="text-xl font-semibold">{title}</h1>
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
                    <AvatarFallback>JD</AvatarFallback>
                  </Avatar>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className="w-56" align="end" forceMount>
                <DropdownMenuLabel className="font-normal">
                  <div className="flex flex-col space-y-1">
                    <p className="text-sm font-medium leading-none">
                      {isUserLoading ? (
                        "Loading..."
                      ) : isUserError ? (
                        "Error loading name"
                      ) : (
                        <>
                          <span className="capitalize">{user!.firstName}</span>{" "}
                          <span className="capitalize">{user!.lastName}</span>
                        </>
                      )}
                    </p>
                    <p className="text-xs leading-none text-muted-foreground">
                      {isUserLoading
                        ? "Loading..."
                        : isUserError
                        ? "Error loading email"
                        : user!.email}
                    </p>
                  </div>
                </DropdownMenuLabel>
                <DropdownMenuSeparator />{" "}
                <DropdownMenuItem>
                  <User className="mr-2 h-4 w-4" />
                  <span>My Profile</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <CreditCard className="mr-2 h-4 w-4" />
                  <span>Billing</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <Settings className="mr-2 h-4 w-4" />
                  <span>Settings</span>
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem onClick={logout}>
                  <LogOut className="mr-2 h-4 w-4" />
                  <span>Log Out</span>
                </DropdownMenuItem>
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
