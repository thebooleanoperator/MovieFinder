import { ApiProvider } from './context/CoreApiContext/CoreApiContext';
import { LoginApiProvider } from './context/LoginApiContext/LoginApiContext';
import { PrestoRouter } from './router/PrestoRouter';

function App() {
  return (
    <ApiProvider>
      <LoginApiProvider>
        <PrestoRouter />
      </LoginApiProvider>
    </ApiProvider>
  );
}

export default App;
