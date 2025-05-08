import { BookFilter } from "@/types/filters/BookFilter";

const AudioBookFilter: React.FC<{
  filters: BookFilter;
  onFilterChange: (filters: BookFilter) => void;
}> = ({ filters, onFilterChange }) => {
  // Спрощена версія — адаптуй під себе
  return (
    <div className="p-4 bg-yellow-400 text-black">
      <p className="font-bold">Фільтри для аудіокниг</p>
    </div>
  );
};

export default AudioBookFilter;
