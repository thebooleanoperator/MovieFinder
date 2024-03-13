import { ApiProvider } from './context/CoreApiContext/CoreApiProvider';
import { LoginApiProvider } from './context/LoginApiContext/LoginApiProvider';
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
