export const dateToLocaleString = (date: Date) => {
  return date.toLocaleString(["en-US", "es-ES"], {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
};

export const isPastDate = (date: Date) => {
  const today = new Date();
  return date < today || date < new Date("1900-01-01");
};
