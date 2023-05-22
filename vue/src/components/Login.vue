<template>
<div id="login">
    <form class="login-form" @submit.prevent="login">
      <h1 >Please Sign In</h1>
      <div role="alert" v-if="invalidCredentials">
        Invalid username and password!
      </div>
      <div role="alert" v-if="this.$route.query.registration">
        Thank you for registering, please sign in.
      </div>
      <div class="form-input-group">
        <label for="username">Username</label>
        <input type="text" id="username" v-model="user.username" required autofocus />
      </div>
      <div class="form-input-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="user.password" required />
      </div>
      <button type="submit">Sign in</button>
      <p>
      <router-link :to="{ name: 'register' }">Need an account? Sign up.</router-link></p>
    </form>
  </div>
  <!-- <div class="login-page" :style="backgroundStyle">
    <div class="login-form" :style="formStyle">
      <h1 class="login-title">{{ title }}</h1>
      <input v-model="username" type="text" class="login-input" placeholder="Username">
      <input v-model="password" type="password" class="login-input" placeholder="Password">
      <button @click="login" class="login-button">{{ buttonText }}</button>
    </div>
  </div> -->
</template>

<script>
export default {
    name: 'login',
  data() {
    return {
      username: '',
      password: '',
      title: 'Login',
      buttonText: 'Login',
      gradientStartColor: '#7f0cf2',
      gradientEndColor: '#ff00dc',
      accentColor: '#ffc700',
      backgroundColor: '#4CBB17'
    };
  },
  computed: {
    backgroundStyle() {
      return {
        background: `linear-gradient(to bottom right, ${this.gradientStartColor}, ${this.gradientEndColor})`
      };
    },
    formStyle() {
      return {
        backgroundColor: this.backgroundColor,
        color: this.accentColor
      };
    }
  },
  methods: {
    login() {
      // Perform login logic here
      // Use this.username and this.password to get the entered values
      // Customize the login logic as needed for your application
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
  font-weight: bold;
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
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  max-width: 400px;
  width: 90%;
}

.login-title {
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
  background-color: #ffc700;
  color: black;
  font-weight: bold;
}
</style>
