<template>
  <div class="landing-page" :style="backgroundStyle">
    <h1 class="landing-title">{{ title }}</h1>
    <div class="active-cars">
      <div v-for="car in activeCars" :key="car.licensePlate" class="car-container" @click="showPickupButton(car)" :style="formStyle">
        <div class="car-info">
          <p class="car-make">{{ car.make }}</p>
          <p class="car-model">{{ car.model }}</p>
          <p class="car-color">{{ car.color }}</p>
          <p class="car-license">{{ car.licensePlate }}</p>
          <p class="car-checkin">{{ car.checkinTime }}</p>
        </div>
        <button v-if="car.showPickupButton" @click="requestPickup(car)" class="pickup-button">Request Pickup</button>
      </div>
      <button @click="logout" class="logout-button">Log Out</button>
    </div>
  </div>
</template>

<script>
export default {
  name: "landing-page",
  computed: {
    backgroundStyle() {
    return {
      background: `linear-gradient(to bottom right, ${this.gradientStartColor}, ${this.gradientEndColor})`};
    },
    formStyle() {
      return {
        backgroundColor: this.backgroundColor,
        color: this.accentColor
      };
    }
  },
  data() {
    return {
      title: 'Active Cars',
      activeCars: [
        { make: 'Toyota', model: 'Camry', color: 'Blue', licensePlate: 'ABC123', checkinTime: '10:00 AM', showPickupButton: false },
        { make: 'Honda', model: 'Accord', color: 'Red', licensePlate: 'DEF456', checkinTime: '11:30 AM', showPickupButton: false },
        { make: 'Ford', model: 'Mustang', color: 'Yellow', licensePlate: 'GHI789', checkinTime: '12:45 PM', showPickupButton: false }
      ],
      gradientStartColor: '#7f0cf2',
      gradientEndColor: '#ff00dc',
      accentColor: '#ffc700',
      backgroundColor: '#4CBB17'
    };
  },
  methods: {
    logout(){
      alert("Logging you out");
    },
    showPickupButton(car) {
      car.showPickupButton = !car.showPickupButton;
    },
    requestPickup(car) {
        alert("Your car is on the way!");
        console.log(car);
      // Perform request pickup logic here
      // Use car.make, car.model, car.color, car.licensePlate, etc. to get car details
      // Customize the request pickup logic as needed for your application
    }
  }
}
</script>

<style scoped>
.landing-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  width: 100%;
}

.landing-title {
  text-align: center;
  margin-bottom: 32px;
}

.active-cars {
  max-width: 800px;
  width: 90%;
}

.car-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  margin-bottom: 16px;
  background-color: #f9f9f9;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: background-color 0.3s ease-in-out;
}

.car-container:hover {
  background-color: #e8e8e8;
}

.car-info {
  flex: 1;
}

.car-make {
  font-weight: bold;
}

.pickup-button {
  padding: 12px 16px;
  border: none;
  border-radius: 4px;
  background-color: #ffc700;
  color: black;
  font-weight: bold;
  cursor: pointer;
}

.logout-button {
  width: 100%;
  padding: 12px 16px;
  border: none;
  border-radius: 4px;
  background-color: #ffc700;
  color: black;
  font-weight: bold;
}
</style>
