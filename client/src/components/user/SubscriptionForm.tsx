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
      setValue('title', existingSubscription.title ?? undefined);
      setValue(
        'expirationDays',
        existingSubscription.expirationDays ?? undefined,
      );
      setValue('price', existingSubscription.price ?? undefined);
      setValue('subdescription', existingSubscription.subdescription ?? undefined);
      setValue('description', existingSubscription.description ?? undefined);
      setImagePreview(existingSubscription.imageUrl ?? undefined);
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

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <label
          htmlFor='imageUpload'
          style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            width: '150px',
            height: '150px',
            border: '3px dashed #ccc',
            borderRadius: '100%',
            cursor: 'pointer',
            overflow: 'hidden',
            backgroundSize: 'cover',
            backgroundPosition: 'center',
            backgroundImage: imagePreview ? `url(${imagePreview})` : 'none',
          }}
        >
          {!imagePreview && <span>Click to Upload</span>}
        </label>
        <input
          id='imageUpload'
          type='file'
          accept='image/*'
          style={{ display: 'none' }}
          onChange={handleImageChange}
        />
        <p>{errors.image?.message}</p>

        <input {...register('title')} placeholder='Title' />
        <p>{errors.title?.message}</p>

        <input
          type='number'
          {...register('expirationDays')}
          placeholder='expirationDays'
        />
        <p>{errors.expirationDays?.message}</p>

        <input type='number' {...register('price')} placeholder='Price' />
        <p>{errors.price?.message}</p>

        <input {...register('subdescription')} placeholder='subdescription' />
        <p>{errors.subdescription?.message}</p>

        <input {...register('description')} placeholder='description' />
        <p>{errors.description?.message}</p>

        <button type='submit' disabled={loading}>
          {existingSubscription ? 'Update Subscription' : 'Add Subscription'}
        </button>
      </form>
    </>
  );
};

export default SubscriptionForm;
