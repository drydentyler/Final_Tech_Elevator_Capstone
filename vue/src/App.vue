<template>
  <div id="app">
    <div id="nav">
      
      <div class="links">
        <router-link
          v-bind:to="{ name: 'logout' }"
          v-if="$store.state.token != ''">Logout</router-link>
          </div>
        <div class="logo">
        <img src='./assets/mustache-taco-small.png'/>
      </div>

        <div id="weather">{{overcast}}
    <span class="temperature"> | {{currentTemp}}°</span><br>
    <span class="temperature"> Min {{minTemp}}° | Max {{maxTemp}}°</span><br>
    <span id="temp-values"></span>
  
  <div id="info">
    {{humidity}} 
{{wind}}
  </div>
  <div>
    <!-- Add available spots here -->
  </div>
  </div>
      
      </div>
    
    
    <router-view />
  </div>
</template>

<script>
import WeatherService from "./services/WeatherService";

export default {
  name: "weather-data",
  data() {
    return {
      currentTemp: '',
      minTemp: '',
      maxTemp:'',
      sunrise: '',
      sunset: '',
      pressure: '',
      humidity: '',
      wind: '',
      overcast: '', 
      icon: '' 
    };
  },
  created() {
    WeatherService.getWeather().then((response) => {
      this.currentTemp =  (1.8*((response.data.main.temp)-273) + 32).toFixed(0);
          this.minTemp = (1.8*((response.data.main.temp_min)-273) + 32).toFixed(0);
          this.maxTemp = (1.8*((response.data.main.temp_max)-273) + 32).toFixed(0);
          this.pressure = response.data.main.pressure;
          this.humidity = response.data.main.humidity + '%';
          this.wind = response.data.wind.speed + 'm/s';
          this.overcast = response.data.weather[0].description;
          this.icon = "images/" + response.data.weather[0].icon.slice(0, 2) + ".svg";
          this.sunrise = new Date(response.data.sys.sunrise*1000).toLocaleTimeString("en-GB").slice(0,4);
          this.sunset = new Date(response.data.sys.sunset*1000).toLocaleTimeString("en-GB").slice(0,4);
    });
  },
  methods: {
    capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
  }
}
</script>


<style scoped>
  @import url('https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100;0,400;0,600;0,700;0,900;1,100;1,400;1,700;1,900&display=swap');


#app {
  background-image: url("assets/goo.png");
  background-size: cover;
  font-family: Montserrat;
  text-align: center;
  min-height: 110vh;
}

#nav {
  width: 100%;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  padding: 0 20px;
  background: rgba(255, 255, 255, 0.1); 
  backdrop-filter: blur(10px); 
  border: 1px solid rgba(255, 255, 255, 0.2);
}

#weather {
  flex: 1;
  text-align: right;
  font-size: 20px;
  border-radius: 4px;
  color: white;
  padding-right: 15px;
}

.links {
  flex: 1;
  text-align: left;
  font-size: 25px;
  color: white;
}

a:visited {
  color: white;
}
a:hover {
  color: #7f0cf2;
}

.logo {
  flex: 2;
  text-align: center;
}
 .logo > img{
  height: 125px;
} 

@media screen and (max-width: 768px) {
  #weather {
    font-size: 15px;
    padding-right: 0px;
  }
}
</style>