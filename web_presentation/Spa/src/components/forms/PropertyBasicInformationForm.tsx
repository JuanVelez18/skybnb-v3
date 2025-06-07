import z from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import type { PropertyBasicInformation } from "@/models/properties";
import { useGetAllPropertyTypes } from "@/queries/properties.queries";

type Props = {
  formId?: string;
  initialValues?: PropertyBasicInformation | null;
  onSubmit: (information: PropertyBasicInformation) => void;
};

const formSchema = z.object({
  title: z
    .string()
    .min(10, "Title must be at least 10 characters long")
    .max(100, "Title cannot exceed 100 characters"),
  description: z
    .string()
    .min(30, "Description must be at least 30 characters long")
    .max(2000, "Description cannot exceed 2000 characters"),
  bathrooms: z.string().min(1, "There must be at least one bathroom"),
  bedrooms: z.string().min(1, "There must be at least one bedroom"),
  beds: z.string().min(1, "There must be at least one bed"),
  maxGuests: z.string().min(1, "There must be at least one guest"),
  basePrice: z.string().min(1, "Base price must be greater than zero"),
  propertyType: z.string().nonempty("You must select a property type"),
});

type FormValues = z.infer<typeof formSchema>;

const modelToFormValues = (model: PropertyBasicInformation): FormValues => ({
  title: model.title,
  description: model.description || "",
  bathrooms: model.bathrooms.toString(),
  bedrooms: model.bedrooms.toString(),
  beds: model.beds.toString(),
  maxGuests: model.maxGuests.toString(),
  basePrice: model.basePrice.toString(),
  propertyType: model.propertyType,
});

const formValuesToModel = (values: FormValues): PropertyBasicInformation => ({
  title: values.title,
  description: values.description || "",
  bathrooms: values.bathrooms ? parseInt(values.bathrooms) : 0,
  bedrooms: values.bedrooms ? parseInt(values.bedrooms) : 0,
  beds: values.beds ? parseInt(values.beds) : 0,
  maxGuests: values.maxGuests ? parseInt(values.maxGuests) : 0,
  basePrice: values.basePrice ? parseFloat(values.basePrice) : 0,
  propertyType: values.propertyType,
});

const PropertyBasicInformationForm = ({
  formId,
  initialValues,
  onSubmit,
}: Props) => {
  const { propertyTypes, arePropertyTypesLoading, isPropertyTypesError } =
    useGetAllPropertyTypes();

  const propertyTypesPlaceholder = arePropertyTypesLoading
    ? "Loading property types..."
    : isPropertyTypesError
    ? "Error loading property types"
    : "Select a property type";

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues
      ? modelToFormValues(initialValues)
      : {
          title: "",
          description: "",
          bathrooms: "",
          bedrooms: "",
          beds: "",
          maxGuests: "",
          basePrice: "",
          propertyType: "",
        },
  });

  const handleSubmit = (data: FormValues) => {
    const formattedData: PropertyBasicInformation = formValuesToModel(data);
    onSubmit(formattedData);
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Basic Information</CardTitle>
        <CardDescription>
          Provide the main details of your property so guests can get to know it
          better.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form
            id={formId}
            onSubmit={form.handleSubmit(handleSubmit)}
            className="space-y-6"
          >
            {" "}
            {/* Título */}
            <FormField
              control={form.control}
              name="title"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Property title *</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="E.g.: Beautiful house with sea view on the coast"
                      {...field}
                    />
                  </FormControl>
                  <FormDescription>
                    Create an attractive title that describes your property
                    (10-100 characters)
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />{" "}
            {/* Descripción */}
            <FormField
              control={form.control}
              name="description"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description *</FormLabel>
                  <FormControl>
                    <Textarea
                      placeholder="Describe your property, its special features, the environment and what makes it unique..."
                      className="min-h-[100px]"
                      {...field}
                    />
                  </FormControl>
                  <FormDescription>
                    Provide additional details that help guests better
                    understand your property
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />{" "}
            {/* Grid para campos numéricos */}
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-4">
              <FormField
                control={form.control}
                name="bedrooms"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Bedrooms *</FormLabel>
                    <FormControl>
                      <Input type="number" min="1" placeholder="1" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="beds"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Beds *</FormLabel>
                    <FormControl>
                      <Input type="number" min="1" placeholder="1" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="bathrooms"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Bathrooms *</FormLabel>
                    <FormControl>
                      <Input type="number" min="1" placeholder="1" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="maxGuests"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Max guests *</FormLabel>
                    <FormControl>
                      <Input type="number" min="1" placeholder="2" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>{" "}
            {/* Tipo de propiedad y precio */}
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              <FormField
                control={form.control}
                name="propertyType"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Property type *</FormLabel>
                    <Select
                      onValueChange={field.onChange}
                      defaultValue={field.value}
                      disabled={arePropertyTypesLoading || isPropertyTypesError}
                    >
                      <FormControl>
                        <SelectTrigger>
                          <SelectValue placeholder={propertyTypesPlaceholder} />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {!arePropertyTypesLoading &&
                          !isPropertyTypesError &&
                          propertyTypes!.map((type) => (
                            <SelectItem key={type.id} value={type.id}>
                              {type.name}
                            </SelectItem>
                          ))}
                      </SelectContent>
                    </Select>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="basePrice"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Base price per night *</FormLabel>
                    <FormControl>
                      <div className="relative">
                        <span className="absolute left-3 top-1/2 -translate-y-1/2 text-muted-foreground">
                          $
                        </span>
                        <Input
                          type="number"
                          min="1"
                          placeholder="50"
                          className="pl-8"
                          {...field}
                        />
                      </div>
                    </FormControl>
                    <FormDescription>
                      Price in USD per night (excluding fees)
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

export default PropertyBasicInformationForm;
