import { useEffect, useState } from 'react';
import { PrestoRouter } from './router/prestoRouter';
import { MainLayout } from './layouts/MainLayout';


function App() {
    const [showLogin, setShowLogin] = useState(false)
    const [showRegister, setShowRegister] = useState(false)

    const setupApp = () => {
        // TODO: check cookie for session
        setShowLogin(true)
        setShowRegister(false)
    }

    useEffect(() => {
        setupApp()
    }, [])
  return (
    <>
    <MainLayout>
      <PrestoRouter />
    </MainLayout>
    </>
  );
}

export default App;
