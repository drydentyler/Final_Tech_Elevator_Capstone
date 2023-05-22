<template>
    <div class="availableSpots">
      <h2 class="full-lot-message" v-show="availableSpots == 0">WARNING! We currently have no spots available!</h2>
      <h2 v-show="availableSpots > 1">We currently have {{ availableSpots }} spots available!</h2>
      </div>
</template>

<style scoped>
.availableSpots {
  color: white;
  font-size: 25px;
  padding-top: 15px;
  padding-bottom: 15px;
}
</style>

<script>
import ParkingLotService from "../services/ParkingLotService";

export default {
    async mounted() {
    await ParkingLotService.getAvailableSpots().then((response) => {
        this.availableSpots = response.data;
      })
  },
  name: "available-spots",
  data() {
      return {
          availableSpots: ''
      }
  }
}
</script>

<style scoped>

.full-lot-message {
  font-size: 40px;
  font-weight: bold; /* Change the size to suit your needs */
  color: red;
  text-align: center;
  /* Add any additional styles you want for the warning sign */
  /*border: 2px solid red;  Add border to the text */
  animation: pulsing 1s infinite; /* Add pulsing animation */
}

@keyframes pulsing {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}

</style>