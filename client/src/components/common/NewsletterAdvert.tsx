import '@/assets/styles/components/common/newsletterAdvert.css'
import { icons } from '@/lib/icons'


const NewsletterAdvert: React.FC = () =>{

  return (
    <div className='advert-container'>
        <div className='flex flex-col gap-3.5'>
            <div className='advert-header'>
                <p><span className='sale'>-15%</span> discount on your first purchase for subscribing to our newsletter</p>
            </div>
            <p>Join our community to receive information about the latest promotions and products</p>
        </div>
        <div>
            <div className='newsletter'>
                <div className='newsletter-input-container'>
                    <img src={icons.bLetter}/>
                    <input className='newsletter-input' type='text' placeholder='Enter your email address.'/>
                </div>
                <button className='newsletter-submit'>Subscribe</button>
            </div>
        </div>
    </div>
  );
};

export default NewsletterAdvert;



