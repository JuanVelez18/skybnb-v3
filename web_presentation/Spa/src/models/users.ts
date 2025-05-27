export type UserSummary = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  createdAt: Date;
  roles: string[];
  permissions: string[];
};
