<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-content">
        <div class="action-item">
          <img src="@/assets/images/rk.png" />
          <a-button type="primary" @click="onRkClick">入库</a-button>
        </div>
        <!-- <div class="action-item">
          <img src="@/assets/images/dh.png" />
          <a-button type="primary">调货</a-button>
        </div> -->
        <div class="action-item">
          <img src="@/assets/images/ck.png" />
          <a-button type="primary" @click="onCkClick">出库</a-button>
        </div>
      </div>
    </template>

    <rk-form ref="rkForm" @onFinish="onRkFinish" />
    <ck-form ref="ckForm" @onFinish="onCkFinish" />
  </page-header-wrapper>
</template>

<script>
// import { getUserProfile } from '@/api/system/user'
import RkForm from './components/rkForm'
import CkForm from './components/ckForm'

export default {
  name: 'Actions',
  components: { RkForm, CkForm },
  data() {
    return {}
  },
  computed: {
    // ...mapGetters(['avatar', 'nickname'])
  },
  created() {},
  mounted() {},
  methods: {
    onRkFinish() {},
    onCkFinish() {},
    onRkClick() {
      this.$refs.rkForm.showModal('')
    },
    onCkClick() {
      this.$refs.ckForm.showModal('')
    }
  }
}
</script>

<style lang="less" scoped>
.page-content {
  display: flex;
  justify-content: space-evenly;
  .action-item {
    display: flex;
    flex-direction: column;
    img {
      width: 160px;
      margin-bottom: 16px;
    }
  }
}
</style>
