<template>
  <div class="container">
    <side-bar></side-bar>
    <div class="content">
      <top-bar></top-bar>
      <div class="view">
        <router-view></router-view>
      </div>
    </div>
  </div>
</template>

<script lang="ts" src="./App"></script>
<style lang="scss">
html {
  height: 100%;
}

body {
  min-height: 100%;

  #nprogress .bar {
    background-color: #065582;
  }

  #nprogress .spinner {
    right: auto;
    left: 15px;
  }

  .error {
    color: #FF4949;
  }
}

.content {
  padding-left: 260px;

  .view {
    margin: 20px;
  }
}

.side-menu{
    display: none;
  }

@media screen and (max-width: 900px) {
  .mobile-hide {
    display: none;
  }
  .content {
    padding-left: 0;
  }
  .side-menu{
    display: block;
  }
}
</style>