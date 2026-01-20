/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'

// Composables
import { createApp } from 'vue'

// GraphQL apollo client
import { createApolloProvider } from '@vue/apollo-option';
import apolloClient from './apollo-client';

const apolloProvider = createApolloProvider({
  defaultClient: apolloClient,
});

// Styles
import 'unfonts.css'

const app = createApp(App)

registerPlugins(app)

app.use(apolloProvider);

app.mount('#app')
