import { useNavigate, useParams, useLocation } from 'react-router-dom';
import SubscriptionFormContainer from '../../../../containers/user/SubscriptionFormContainer';

const SubscriptionFormPage: React.FC = () => {
  const { subscriptionId } = useParams<{ subscriptionId: string }>();
  const location = useLocation();
  const navigate = useNavigate();

  const isEditMode = new URLSearchParams(location.search).get('edit') === 'true';
  const isCreating = !subscriptionId;

  const handleGoBack = () => {
    navigate('/admin/subscriptions');
  };

  return (
    <div style={{ maxWidth: '800px', margin: '0 auto', padding: '2rem' }}>
      <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h1 style={{ fontSize: '1.75rem', fontWeight: 'bold' }}>
          {isCreating 
            ? 'Add Subscription' 
            : isEditMode 
            ? 'Edit Subscription' 
            : 'Subscription'}
        </h1>
        <button 
          onClick={handleGoBack} 
          style={{
            padding: '0.5rem 1rem',
            backgroundColor: '#f3f4f6',
            border: '1px solid #ccc',
            borderRadius: '4px',
            cursor: 'pointer'
          }}
        >
          Back to Subscription List
        </button>
      </header>
      <main style={{ marginTop: '2rem' }}>
        <SubscriptionFormContainer id={subscriptionId ?? ''} 
          isEditMode={isEditMode || isCreating}
          isCreating={isCreating}/>
      </main>
    </div>
  );
};

export default SubscriptionFormPage;
