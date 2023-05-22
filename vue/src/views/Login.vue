<template>
<div>
<available-spots />
  <div id="login">
    <form class="login-form" @submit.prevent="login">
      <h1 class="login-title" >Please Sign In</h1>
      <div role="alert" v-if="invalidCredentials">
        Invalid username and password!
      </div>
      <div role="alert" v-if="this.$route.query.registration">
        Thank you for registering, please sign in.
      </div>
      <div class="form-input-group">
        <label for="username">Username</label>
        <input class="login-input" type="text" id="username" v-model="user.username" required autofocus />
      </div>
      <div class="form-input-group">
        <label for="password">Password</label>
        <input class="login-input" type="password" id="password" v-model="user.password" required />
      </div>
      <button class="login-button" type="submit">Sign in</button>
      <p>
      <router-link :to="{ name: 'register' }">Need an account? Sign up.</router-link></p>
    </form>
  </div>
</div>
</template>

<script>
import authService from "../services/AuthService";
import AvailableSpots from "../components/AvailableSpots.vue";

export default {
  name: "login",
  components: {
    AvailableSpots
  },
  data() {
    return {
      user: {
        username: "",
        password: "",
        role: ""
      },
      title: 'Login',
      buttonText: 'Login',
      gradientStartColor: '#7f0cf2',
      gradientEndColor: '#ff00dc',
      accentColor: '#ffc700',
      backgroundColor: '#4CBB17',
      invalidCredentials: false
    };
  },
  methods: {
    login() {
      authService
        .login(this.user)
        .then(response => {
          if (response.status == 200) {
            this.$store.commit("SET_AUTH_TOKEN", response.data.token);
            this.$store.commit("SET_USER", response.data.user);
            if(response.data.user.role == "admin"){
              this.$router.push("/admin");
            }
            if(response.data.user.role == "user"){
              this.$router.push("/");
            }
          }
        })
        .catch(error => {
          const response = error.response;

          if (response.status === 401) {
            this.invalidCredentials = true;
          }
        });
    }
  }
};
</script>

<style scoped>
.form-input-group {
  margin-bottom: 1rem;
}
label {
  margin-right: 0.5rem;
  font-size: 1em;
  font-weight: normal;
  text-align: center;
  margin-bottom: 32px;
}
#login {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  
}

.login-form {
  padding: 32px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  max-width: 400px;
  width: 90%;
  color: white;
  background: rgba(255, 255, 255, 0.1); 
  border-radius: 10px; 
  backdrop-filter: blur(3px); 
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.login-title {
  font-size: 2em;
  font-weight: 100;
  text-align: center;
  margin-bottom: 32px;
}

.login-input {
  width: 100%;
  padding: 12px 16px;
  margin-bottom: 16px;
  border: none;
  border-radius: 4px;
}

.login-button {
  width: 100%;
  padding: 12px 16px;
  border: none;
  border-radius: 4px;
  background-color: #7f0cf2;
  color: white;
  font-weight: normal;
  font-size: 15px;
}
.login-button:hover {
  background-image: linear-gradient(to bottom right, #7f0cf2, dodgerblue);
}

</style>