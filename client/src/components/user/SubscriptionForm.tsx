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
      <form 
        onSubmit={handleSubmit(onSubmit)} 
        style={{
          display: 'flex',
          flexDirection: 'column',
          gap: '1rem',
          background: '#fff',
          padding: '2rem',
          borderRadius: '8px',
          boxShadow: '0 0 10px rgba(0,0,0,0.05)'
        }}
      >
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
            margin: '0 auto'
          }}
        >
          {!imagePreview && <span style={{ color: '#888' }}>Click to Upload</span>}
        </label>
        <input
          id='imageUpload'
          type='file'
          accept='image/*'
          style={{ display: 'none' }}
          onChange={handleImageChange}
        />
        {errors.image && <p style={{ color: 'red' }}>{errors.image.message}</p>}

        <input {...register('title')} placeholder='Title' style={inputStyle} />
        {errors.title && <p style={errorStyle}>{errors.title.message}</p>}

        <input
          type='number'
          {...register('expirationDays')}
          placeholder='Expiration Days'
          style={inputStyle}
        />
        {errors.expirationDays && <p style={errorStyle}>{errors.expirationDays.message}</p>}

        <input
          type='number'
          {...register('price')}
          placeholder='Price'
          style={inputStyle}
        />
        {errors.price && <p style={errorStyle}>{errors.price.message}</p>}

        <input {...register('subdescription')} placeholder='Subdescription' style={inputStyle} />
        {errors.subdescription && <p style={errorStyle}>{errors.subdescription.message}</p>}

        <input {...register('description')} placeholder='Description' style={inputStyle} />
        {errors.description && <p style={errorStyle}>{errors.description.message}</p>}

        <button 
          type='submit' 
          disabled={loading}
          style={{
            padding: '0.75rem',
            backgroundColor: '#2563eb',
            color: '#fff',
            border: 'none',
            borderRadius: '6px',
            cursor: loading ? 'not-allowed' : 'pointer',
            fontWeight: 'bold'
          }}
        >
          {existingSubscription ? 'Update Subscription' : 'Add Subscription'}
        </button>
      </form>

    </>
  );
};

export default SubscriptionForm;
