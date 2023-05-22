import axios from 'axios';

const http = axios.create({
    baseURL: "http://api.openweathermap.org"
});


export default {

    getWeather() {
        return http.get('/data/2.5/weather?lat=40.44&lon=-80.00&appid=9a7080665140cc79d425c25ee22bf244');
    }

}


/* const axios = require('axios');

const apiKey = '9a7080665140cc79d425c25ee22bf244';
const city = 'Pittsburgh';
const state = 'PA';
const apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=${city},${state}&appid=${apiKey}`;

axios.get(apiUrl)
  .then(response => {
    const weatherData = response.data;
    console.log(weatherData);
  })
  .catch(error => {
    console.log(error);
  }); */