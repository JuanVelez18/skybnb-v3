export const dateToLocaleString = (date: Date) => {
  return date.toLocaleString(["en-US", "es-ES"], {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
};

export const isPastDate = (date: Date) => {
  const today = new Date();
  return date < today;
};

export const dateToDateOnlyString = (date: Date): string => {
  return date.toISOString().split("T")[0];
};
