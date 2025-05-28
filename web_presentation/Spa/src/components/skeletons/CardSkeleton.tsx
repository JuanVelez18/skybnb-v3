import { Skeleton } from "@/components/ui/skeleton";

type Props = {
  ref?: React.Ref<HTMLDivElement>;
};

const SkeletonCard = ({ ref }: Props) => {
  return (
    <div className="w-full flex flex-col space-y-3" ref={ref}>
      <Skeleton className="h-[200px] w-full rounded-xl" />
      <div className="space-y-4">
        <Skeleton className="h-4 w-[250px]" />
        <Skeleton className="h-4 w-[200px]" />
      </div>
    </div>
  );
};

export default SkeletonCard;
