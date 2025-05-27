import { ChevronRight } from "lucide-react";

type Step = {
  id: number;
  title: string;
  description: string;
};

type Props = {
  steps: Step[];
  currentStep: number;
};

const Stepper = ({ steps, currentStep }: Props) => {
  return (
    <div className="flex items-center justify-between">
      {steps.map((step, index) => (
        <div key={step.id} className="flex items-center">
          <div className="flex items-center">
            <div
              className={`flex h-10 w-10 items-center justify-center rounded-full border-2 ${
                step.id === currentStep
                  ? "border-primary bg-primary text-primary-foreground"
                  : step.id < currentStep
                  ? "border-primary bg-primary text-primary-foreground"
                  : "border-muted-foreground text-muted-foreground"
              }`}
            >
              {step.id}
            </div>
            <div className="ml-3">
              <p
                className={`text-sm font-medium ${
                  step.id === currentStep
                    ? "text-primary"
                    : "text-muted-foreground"
                }`}
              >
                {step.title}
              </p>
              <p className="text-xs text-muted-foreground">
                {step.description}
              </p>
            </div>
          </div>
          {index < steps.length - 1 && (
            <ChevronRight className="mx-4 h-5 w-5 text-muted-foreground" />
          )}
        </div>
      ))}
    </div>
  );
};

export default Stepper;
