<template>
    <el-card class="box-card" :style="{ height: screenHeight + 'px' }">
        <div slot="header" class="clearfix">
            <span>组织机构</span>
            <el-button
                style="float: right; padding: 3px 0;"
                type="text"
                :icon="expandIcon"
                @click="expandTree"
            >
                {{ expandText }}
            </el-button>
        </div>
        <div :style="{ height: cardBodyHeight + 'px', overflow: 'auto' }">
            <el-tree
                ref="orgTree"
                :data="treeData"
                :props="defaultProps"
                :default-expand-all="isExpand"
                node-key="id"
                @node-click="handleNodeClick"
            />
        </div>
    </el-card>
</template>
<script>
export default {
    data() {
        return {
            props: {
                isRoot: { type: Boolean, default: false } // 是否显示根目录
            },
            treeData: [],
            defaultProps: {
                children: 'children',
                label: 'label',
                id: 'id'
            },
            cardStyle: null,
            screenHeight: null,
            cardBodyHeight: null,
            isExpand: false, // 树是否展开
            expandIcon: 'el-icon-top',
            expandText: '全部展开'
        }
    },
    watch: {
        screenHeight(val) {
            // 为了避免频繁触发resize函数导致页面卡顿，使用定时器
            if (!this.timer) {
                // 一旦监听到的screenWidth值改变，就将其重新赋给data里的screenWidth
                this.screenHeight = val
                this.timer = true
                let that = this
                setTimeout(function() {
                    // 打印screenWidth变化的值
                    that.timer = false
                }, 400)
            }
        },
        isExpand(val) {
            this.expandIcon = val ? 'el-icon-bottom' : 'el-icon-top'
            this.expandText = val ? '全部展开' : '全部收起'
        }
    },
    mounted() {
        const that = this
        that.loadOrgTree()

        window.addEventListener('resize', () => {
            //  resize:当调整浏览器窗口的大小时触发事件
            window.screenHeight = document.documentElement.clientHeight
            that.screenHeight = window.screenHeight - 73
        })
        that.screenHeight = document.documentElement.clientHeight - 73
        that.cardBodyHeight = that.screenHeight - 98
    },
    methods: {
        handleNodeClick(data) {
            this.$emit('tree-node-click', data)
        },
        loadOrgTree() {
            this.$api
                .get('api/system/getorgtree', {
                    params: { isRoot: this.isRoot }
                })
                .then(res => {
                    this.treeData = res.data
                })
        },
        expandTree() {
            for (
                var i = 0;
                i < this.$refs.orgTree.store._getAllNodes().length;
                i++
            ) {
                this.$refs.orgTree.store._getAllNodes()[i].expanded = !this
                    .isExpand
            }
            this.isExpand = !this.isExpand
        }
    }
}
</script>
<style scoped>
@charset "utf-8";
::-webkit-scrollbar {
    width: 5px;
}

/* 定义滚动条轨道 内阴影+圆角 */
::-webkit-scrollbar-track {
    border-radius: 10px;
    background-color: rgba(0, 0, 0, 0.1);
}

/* 定义滑块 内阴影+圆角 */
::-webkit-scrollbar-thumb {
    border-radius: 10px;
    -webkit-box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.3);
    background-color: rgba(0, 0, 0, 0.1);
}
</style>
