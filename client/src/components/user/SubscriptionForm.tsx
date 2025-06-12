import { useForm } from 'react-hook-form';
import { Subscription } from '../../types';
import { SubscriptionFormData, subscriptionSchema } from '../../utils';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';

interface SubscriptionFormProps {
  existingSubscription?: Subscription;
  onAddSubscription: (user: FormData) => Promise<void>;
  onEditSubscription: (updatedSubscription: FormData) => Promise<void>;
  loading: boolean;
  isEditMode?: boolean;
  isCreating?: boolean;
}

const SubscriptionForm: React.FC<SubscriptionFormProps> = ({
  existingSubscription,
  onAddSubscription,
  onEditSubscription,
  loading,
}) => {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<SubscriptionFormData>({
    resolver: zodResolver(subscriptionSchema),
    defaultValues: {
      title: '',
      expirationDays: 14,
      price: 0,
      description: '',
    },
  });

  const [imagePreview, setImagePreview] = useState<string | null>(null);

  useEffect(() => {
    if (existingSubscription) {
      setValue('title', existingSubscription.title ?? '');
      setValue(
        'expirationDays',
        existingSubscription.expirationDays ?? 0,
      );
      setValue('price', existingSubscription.price ?? 0);
      setValue('subdescription', existingSubscription.subdescription ?? '');
      setValue('description', existingSubscription.description ?? '');
      setImagePreview(existingSubscription.imageUrl ?? null);
    }
  }, [existingSubscription, setValue]);

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const imageUrl = URL.createObjectURL(file);
      setImagePreview(imageUrl);
      setValue('image', file);
    }
  };

  const onSubmit = (data: SubscriptionFormData) => {
    const formData = new FormData();
    formData.append(
      'id',
      existingSubscription?.id ?? '00000000-0000-0000-0000-000000000000',
    );
    formData.append('title', data.title);
    formData.append('expirationDays', data.expirationDays.toString());
    formData.append('price', data.price.toString());
    formData.append('subdescription', data.subdescription ?? '');
    formData.append('description', data.description ?? '');
    formData.append('image', data.image ?? '');
    if (existingSubscription)
      formData.append('imageUrl', existingSubscription.imageUrl ?? '');

    if (existingSubscription) onEditSubscription(formData);
    else onAddSubscription(formData);
  };

  const inputStyle = {
    padding: '0.5rem',
    fontSize: '1rem',
    borderRadius: '4px',
    border: '1px solid #ccc',
  };

  const errorStyle = {
    color: 'red',
    fontSize: '0.875rem',
    marginTop: '-0.5rem'
  };

  return (
    <>
    <div className="max-w-4xl mx-auto p-6 bg-[#f9f5ec] min-h-screen">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-[#dd4a4a]">Нова підписка</h1>
        <div className="space-x-2">
          <button className="bg-[#fbd949] text-black font-bold px-4 py-2 rounded shadow">
            Створити нову
          </button>
        </div>
      </div>

      <div className="flex flex-col items-center">
        <div className="w-32 h-32 rounded-full bg-gray-200 text-gray-500 flex items-center justify-center text-center text-sm font-medium mb-6 overflow-hidden relative cursor-pointer">
          {imagePreview ? (
            <img src={imagePreview} alt="Preview" className="w-full h-full object-cover rounded-full" />
          ) : (
            <span>Натисніть<br />щоб додати<br />зображення</span>
          )}
          <input
            type="file"
            accept="image/*"
            onChange={handleImageChange}
            className="absolute inset-0 opacity-0 cursor-pointer"
          />
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 w-full">
          <div className="bg-white p-4 rounded shadow space-y-4">
            <div>
              <label className="block text-sm font-semibold mb-1">
                Термін дії підписки (днів)
              </label>
              <input
                {...register("expirationDays")}
                type="number"
                className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-[#dd4a4a]"
                placeholder="Наприклад: 365"
              />
            </div>
            <div>
              <label className="block text-sm font-semibold mb-1">Ціна (грн)</label>
              <input
                {...register("price")}
                type="number"
                className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-[#dd4a4a]"
                placeholder="Наприклад: 365"
              />
            </div>
          </div>

          <div className="bg-white p-4 rounded shadow space-y-4">
            <div>
              <label className="block text-sm font-semibold mb-1">Опис скорочено</label>
              <input
                {...register("subdescription")}
                type="text"
                className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-[#dd4a4a]"
                placeholder="Короткий опис"
              />
            </div>
            <div>
              <label className="block text-sm font-semibold mb-1">Опис повний</label>
              <textarea
                {...register("description")}
                className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-[#dd4a4a]"
                placeholder="Повний опис"
                rows={4}
              />
            </div>
          </div>
        </div>

        <button
          type="submit"
          className="mt-6 bg-[#2563eb] text-white font-semibold px-6 py-3 rounded shadow hover:bg-blue-700 transition"
        >
          Зберегти підписку
        </button>
      </div>
    </div>
    </>
  );
};

export default SubscriptionForm;
