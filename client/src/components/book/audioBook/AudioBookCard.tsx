import { Book } from "@/types";

const AudioBookCard: React.FC<{ item: Book }> = ({ item }) => {
  return (
    <div className="audio-card">
      <img src={item.audioFileUrl} alt={item.title} className="w-full h-60 object-cover" />
      <div className="p-2">
        <h3 className="font-bold">{item.title}</h3>
        <p className="text-sm">{item.authorId}</p>
        <p className="text-yellow-600 font-semibold">{item.price} UAH</p>
        <button className="mt-2 bg-yellow-400 px-3 py-1 rounded">BUY</button>
      </div>
    </div>
  );
};

export default AudioBookCard;
    