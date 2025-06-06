import { ChevronLeft, ChevronRight } from "lucide-react";
import { useState, type ReactNode } from "react";
import { useNavigate } from "react-router-dom";

import Stepper from "@/components/common/Stepper";
import { Button } from "@/components/ui/button";
import { PropertyBasicInformationForm } from "@/components/forms";
import type { PropertyBasicInformation } from "@/models/properties";
import PropertyAddressForm from "@/components/forms/PropertyAddressForm";
import type { CreationAddress } from "@/models/ubication";
import type { CreationMediaFile } from "@/models/multimedia";
import PropertyMultimediaForm from "@/components/forms/PropertyMultimediaForm";
import { useCreateProperty } from "@/queries/properties.queries";
import { RouteNames } from "@/router/routes";
import Loader from "@/components/common/Loader";

const STEPS = {
  BASIC_INFORMATION: 1,
  ADDRESS: 2,
  MULTIMEDIA: 3,
} as const;

const steps = [
  {
    id: STEPS.BASIC_INFORMATION,
    title: "Basic Information",
    description: "Main details of your property",
  },
  {
    id: STEPS.ADDRESS,
    title: "Address",
    description: "Location of your property",
  },
  {
    id: STEPS.MULTIMEDIA,
    title: "Multimedia",
    description: "Photos and videos of your property",
  },
];

const getFormId = (step: number): string => {
  return `step-creation-${step}`;
};

const PropertyCreationPage = () => {
  const navigate = useNavigate();
  const { createProperty, isCreatingProperty } = useCreateProperty();

  const [currentStep, setCurrentStep] = useState<number>(
    STEPS.BASIC_INFORMATION
  );
  const [propertyData, setPropertyData] = useState({
    [STEPS.BASIC_INFORMATION]: null as PropertyBasicInformation | null,
    [STEPS.ADDRESS]: null as CreationAddress | null,
    [STEPS.MULTIMEDIA]: null as CreationMediaFile[] | null,
  });

  const handlePreviousStep = () => {
    if (currentStep === STEPS.BASIC_INFORMATION) return;
    setCurrentStep((prev) => prev - 1);
  };

  const handleBasicInformationSubmit = (data: PropertyBasicInformation) => {
    setPropertyData((prev) => ({
      ...prev,
      [STEPS.BASIC_INFORMATION]: data,
    }));
    setCurrentStep(STEPS.ADDRESS);
  };

  const handleAddressSubmit = (data: CreationAddress) => {
    setPropertyData((prev) => ({
      ...prev,
      [STEPS.ADDRESS]: data,
    }));
    setCurrentStep(STEPS.MULTIMEDIA);
  };

  const handleMultimediaSubmit = (data: CreationMediaFile[]) => {
    setPropertyData((prev) => ({
      ...prev,
      [STEPS.MULTIMEDIA]: data,
    }));

    createProperty(
      {
        information: propertyData[STEPS.BASIC_INFORMATION]!,
        address: propertyData[STEPS.ADDRESS]!,
        multimedia: data,
      },
      {
        onSuccess() {
          navigate(RouteNames.HOME);
        },
      }
    );
  };

  let Form: ReactNode;
  switch (currentStep) {
    case STEPS.BASIC_INFORMATION:
      Form = (
        <PropertyBasicInformationForm
          formId={getFormId(STEPS.BASIC_INFORMATION)}
          initialValues={propertyData[STEPS.BASIC_INFORMATION]}
          onSubmit={handleBasicInformationSubmit}
        />
      );
      break;
    case STEPS.ADDRESS:
      Form = (
        <PropertyAddressForm
          formId={getFormId(STEPS.ADDRESS)}
          initialValues={propertyData[STEPS.ADDRESS]}
          onSubmit={handleAddressSubmit}
        />
      );
      break;
    case STEPS.MULTIMEDIA:
      Form = (
        <PropertyMultimediaForm
          formId={getFormId(STEPS.MULTIMEDIA)}
          initialValues={propertyData[STEPS.MULTIMEDIA]}
          onSubmit={handleMultimediaSubmit}
        />
      );
      break;
    default:
      Form = <div>Unknown Step</div>;
  }

  return (
    <div className="mx-auto max-w-4xl space-y-6">
      <Stepper steps={steps} currentStep={currentStep} />

      {Form}

      <div className="flex justify-between pt-6">
        <Button
          type="button"
          variant="outline"
          onClick={handlePreviousStep}
          disabled={currentStep === STEPS.BASIC_INFORMATION}
        >
          <ChevronLeft className="mr-2 h-4 w-4" />
          Previous
        </Button>

        <Button
          type="submit"
          form={getFormId(currentStep)}
          className="cursor-pointer"
        >
          {currentStep === STEPS.MULTIMEDIA ? "Create Property" : "Next"}
          <ChevronRight className="ml-2 h-4 w-4" />
        </Button>
      </div>

      {isCreatingProperty && <Loader />}
    </div>
  );
};

export default PropertyCreationPage;
