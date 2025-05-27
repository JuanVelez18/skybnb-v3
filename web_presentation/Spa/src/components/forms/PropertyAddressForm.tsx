import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import z from "zod";

import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from "@/components/ui/card";
import {
  Form,
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
  FormDescription,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectTrigger,
  SelectContent,
  SelectItem,
  SelectValue,
} from "@/components/ui/select";
import type { CreationAddress } from "@/models/ubication";
import { useGetAllCities } from "@/queries/ubication.queries";

type Props = {
  formId?: string;
  initialValues?: CreationAddress | null;
  onSubmit: (address: CreationAddress) => void;
};

const addressFormSchema = z.object({
  street: z.string().min(1, {
    message: "Street name is required.",
  }),
  streetNumber: z.string().min(1, {
    message: "Street number is required.",
  }),
  intersectionNumber: z.string().min(1, {
    message: "Intersection number is required.",
  }),
  doorNumber: z.string().min(1, {
    message: "Door number is required.",
  }),
  city: z.string().min(1, {
    message: "Please select a city.",
  }),
  complement: z
    .string()
    .max(200, {
      message: "Complement must be 200 characters or less.",
    })
    .optional(),
  latitude: z.string().optional(),
  longitude: z.string().optional(),
});

type AddressFormValues = z.infer<typeof addressFormSchema>;

const modelToFormValues = (address: CreationAddress): AddressFormValues => ({
  street: address.street,
  streetNumber: address.streetNumber.toString(),
  intersectionNumber: address.intersectionNumber.toString(),
  doorNumber: address.doorNumber.toString(),
  city: address.cityId.toString(),
  complement: address.complement || "",
  latitude: address.latitude ? address.latitude.toString() : "",
  longitude: address.longitude ? address.longitude.toString() : "",
});

const formValuesToModel = (values: AddressFormValues): CreationAddress => ({
  street: values.street,
  streetNumber: values.streetNumber ? parseInt(values.streetNumber, 10) : 0,
  intersectionNumber: values.intersectionNumber
    ? parseInt(values.intersectionNumber, 10)
    : 0,
  doorNumber: values.doorNumber ? parseInt(values.doorNumber, 10) : 0,
  cityId: values.city ? parseInt(values.city, 10) : 0,
  complement: values.complement || "",
  latitude: values.latitude ? parseFloat(values.latitude) : null,
  longitude: values.longitude ? parseFloat(values.longitude) : null,
});

const PropertyAddressForm = ({ formId, initialValues, onSubmit }: Props) => {
  const { cities, areCitiesLoading, isCitiesError } = useGetAllCities();

  const addressForm = useForm<AddressFormValues>({
    resolver: zodResolver(addressFormSchema),
    defaultValues: initialValues
      ? modelToFormValues(initialValues)
      : {
          street: "",
          streetNumber: "",
          intersectionNumber: "",
          doorNumber: "",
          city: "",
          complement: "",
          latitude: "",
          longitude: "",
        },
  });

  const citiesPlaceholder = areCitiesLoading
    ? "Loading cities..."
    : isCitiesError
    ? "Error loading cities"
    : "Select a city";

  const handleSubmit = (values: AddressFormValues) => {
    const address = formValuesToModel(values);
    onSubmit(address);
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Address</CardTitle>
        <CardDescription>
          Provide the complete address where your property is located.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...addressForm}>
          <form
            id={formId}
            onSubmit={addressForm.handleSubmit(handleSubmit)}
            className="space-y-6"
          >
            {/* Street and Street Number */}
            <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
              <div className="md:col-span-2">
                <FormField
                  control={addressForm.control}
                  name="street"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Street *</FormLabel>
                      <FormControl>
                        <Input placeholder="e.g. Main Street" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <FormField
                control={addressForm.control}
                name="streetNumber"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Street Number *</FormLabel>
                    <FormControl>
                      <Input placeholder="e.g. 123" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            {/* Intersection and Door Number */}
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              <FormField
                control={addressForm.control}
                name="intersectionNumber"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Intersection Number *</FormLabel>
                    <FormControl>
                      <Input placeholder="e.g. 456" {...field} />
                    </FormControl>
                    <FormDescription>
                      Cross street or intersection reference number
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={addressForm.control}
                name="doorNumber"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Door Number *</FormLabel>
                    <FormControl>
                      <Input placeholder="e.g. Apt 4B" {...field} />
                    </FormControl>
                    <FormDescription>
                      Apartment, suite, or unit number
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            {/* City and Complement */}
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              <FormField
                control={addressForm.control}
                name="city"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>City *</FormLabel>
                    <Select
                      onValueChange={field.onChange}
                      defaultValue={field.value}
                    >
                      <FormControl>
                        <SelectTrigger>
                          <SelectValue placeholder={citiesPlaceholder} />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {!areCitiesLoading &&
                          !isCitiesError &&
                          cities!.map((city) => (
                            <SelectItem
                              key={city.id}
                              value={city.id.toString()}
                            >
                              {city.name}
                            </SelectItem>
                          ))}
                      </SelectContent>
                    </Select>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={addressForm.control}
                name="complement"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Address Complement (Optional)</FormLabel>
                    <FormControl>
                      <Input placeholder="e.g. Near Central Park" {...field} />
                    </FormControl>
                    <FormDescription>
                      Additional address information or landmarks
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            {/* Coordinates */}
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              <FormField
                control={addressForm.control}
                name="latitude"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Latitude (Optional)</FormLabel>
                    <FormControl>
                      <Input
                        type="number"
                        step="any"
                        placeholder="e.g. 40.7128"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      GPS coordinate for precise location
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={addressForm.control}
                name="longitude"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Longitude (Optional)</FormLabel>
                    <FormControl>
                      <Input
                        type="number"
                        step="any"
                        placeholder="e.g. -74.0060"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      GPS coordinate for precise location
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
};

export default PropertyAddressForm;
