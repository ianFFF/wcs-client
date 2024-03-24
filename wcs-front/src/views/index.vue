<template>
  <page-header-wrapper>
    <template v-slot:content>
      <div class="page-header-content"></div>
    </template>
    <template v-slot:extraContent> <div></div></template>
    <div></div>
  </page-header-wrapper>
</template>

<script>
import { mapGetters } from 'vuex'
import { PageHeaderWrapper } from '@/components/ProLayout'
// import { getUserProfile } from '@/api/system/user'

export default {
  name: 'Index',
  components: {
    PageHeaderWrapper
  },
  data() {
    return {}
  },
  computed: {
    ...mapGetters(['avatar', 'nickname'])
  },
  created() {
    // this.getUser()
    this.$router.replace('/demo')
  },
  mounted() {},
  methods: {
    // 获取用户信息
    // getUser() {
    //   getUserProfile().then((response) => {
    //     this.user = response.data
    //     this.roleGroup = response.roleGroup
    //     this.postGroup = response.postGroup
    //   })
    // },
    onSponsorTabChange(key, type) {
      this[type] = key
    }
  }
}
</script>

<style lang="less" scoped>
@import './index.less';
blockquote {
  padding: 0 1em;
  color: #6a737d;
  border-left: 0.25em solid #dfe2e5;
}
.project-list {
  .card-title {
    font-size: 0;

    a {
      color: rgba(0, 0, 0, 0.85);
      margin-left: 12px;
      line-height: 24px;
      height: 24px;
      display: inline-block;
      vertical-align: top;
      font-size: 14px;

      &:hover {
        color: #1890ff;
      }
    }
  }

  .card-description {
    color: rgba(0, 0, 0, 0.45);
    height: 66px;
    line-height: 22px;
    overflow: hidden;
  }

  .project-item {
    display: flex;
    margin-top: 8px;
    overflow: hidden;
    font-size: 12px;
    height: 20px;
    line-height: 20px;

    a {
      color: rgba(0, 0, 0, 0.45);
      display: inline-block;
      flex: 1 1 0;

      &:hover {
        color: #1890ff;
      }
    }

    .download {
      color: rgba(0, 0, 0, 0.25);
      flex: 0 0 auto;
      float: right;
    }
  }

  .ant-card-meta-description {
    color: rgba(0, 0, 0, 0.45);
    height: 44px;
    line-height: 22px;
    overflow: hidden;
  }
}

.item-group {
  padding: 20px 0 8px 24px;
  font-size: 0;

  a {
    color: rgba(0, 0, 0, 0.65);
    display: inline-block;
    font-size: 14px;
    margin-bottom: 13px;
    width: 25%;
  }
}

.mobile {
  .project-list {
    .project-card-grid {
      width: 100%;
    }
  }

  .more-info {
    border: 0;
    padding-top: 16px;
    margin: 16px 0 16px;
  }

  .headerContent .title .welcome-text {
    display: none;
  }
}
</style>
