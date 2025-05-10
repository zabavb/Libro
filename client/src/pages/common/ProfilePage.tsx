import Footer from '@/components/layouts/Footer';
import Header from '@/components/layouts/Header';
import ProfileContainer from '@/containers/common/ProfileContainer';

const ProfilePage: React.FC = () => {
    return (
        <div className="flex flex-col min-h-screen">
            <Header />
            <main className="flex-1 overflow-auto">
                <ProfileContainer />
            </main>
            <Footer />
        </div>
    )
}
export default ProfilePage;
