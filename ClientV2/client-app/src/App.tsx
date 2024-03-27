import { ApiProvider } from './context/CoreApiContext/CoreApiProvider';
import { RootProvider } from './context/RootProvider';
import { PrestoRouter } from './router/PrestoRouter';

function App() {
  return (
    <ApiProvider>
      <RootProvider>
        <PrestoRouter />
      </RootProvider>
    </ApiProvider>
  );
}

export default App;
