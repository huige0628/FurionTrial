<template>
    <div class="page-main">
        <el-row :gutter="6">
            <el-col :span="10">
                <el-card class="box-card" :style="{ height: screenHeight + 'px' }">
                    <div :style="{ height: cardBodyHeight + 'px', overflow: 'auto' }">
                        <el-table
                            :data="moduleTreeData"
                            style="width: 100%; margin-bottom: 20px;"
                            row-key="id"
                            border
                            default-expand-all
                            :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
                        >
                            <el-table-column prop="date" label="模块名称" width="240">
                                <template slot-scope="scope">
                                    <el-radio
                                        v-show="scope.row.level == 3"
                                        v-model="filter.moduleId"
                                        :label="scope.row.id"
                                        @change="radChange(scope.row.id)"
                                    >
                                        {{ scope.row.title }}
                                    </el-radio>
                                    <span v-show="scope.row.level != 3">
                                        {{ scope.row.title }}
                                    </span>
                                </template>
                            </el-table-column>
                            <el-table-column prop="code" label="模块编码" width="180" />
                            <el-table-column prop="path" label="模块地址" />
                        </el-table>
                    </div>
                </el-card>
            </el-col>
            <el-col :span="14">
                <!-- head -->
                <SimpleHead
                    :model="filter"
                    @on-search="reloadTable(true)"
                    @on-expand="onExpand"
                >
                    <template slot="button">
                        <!-- 操作按钮 -->
                        <el-form-item>
                            <el-button-group>
                                <el-button
                                    type="success"
                                    icon="el-icon-plus"
                                    @click="moduleAdd"
                                >
                                    添加模块
                                </el-button>
                                <el-button
                                    type="primary"
                                    icon="el-icon-edit"
                                    @click="moduleEdit"
                                >
                                    编辑模块
                                </el-button>
                                <el-button
                                    type="danger"
                                    icon="el-icon-delete"
                                    @click="moduleRemove"
                                >
                                    删除模块
                                </el-button>
                            </el-button-group>
                            <el-button-group>
                                <el-button
                                    type="success"
                                    icon="el-icon-plus"
                                    @click="elementAdd"
                                >
                                    添加按钮
                                </el-button>
                            </el-button-group>
                        </el-form-item>
                    </template>
                    <!-- 默认搜索项 -->
                    <template slot="more">
                        <!-- 更多搜索项 -->
                    </template>
                </SimpleHead>
                <!-- table -->
                <SimpleTable ref="table" :url="tableUrl" :filter="filter" :offset="70">
                    <el-table-column type="selection" width="40" fixed="left" />
                    <el-table-column prop="elementId" label="ID" width="45" />
                    <el-table-column prop="elementName" label="名称" />
                    <el-table-column prop="sortNo" label="排序" />>
                    <el-table-column prop="class" label="样式" />>
                    <el-table-column prop="icon" label="图标" />>
                    <el-table-column prop="operate" label="操作" width="120">
                        <template slot-scope="scope">
                            <el-button-group>
                                <el-button
                                    type="primary"
                                    icon="el-icon-edit"
                                    circle
                                    size="mini"
                                    title="编辑"
                                    @click="edit(scope.row)"
                                />
                                <el-button
                                    type="danger"
                                    icon="el-icon-delete"
                                    circle
                                    size="mini"
                                    title="删除"
                                    @click="remove(scope.row)"
                                />
                            </el-button-group>
                        </template>
                    </el-table-column>
                </SimpleTable>
            </el-col>
        </el-row>
        <ModuleAddEdit ref="moduleEdit" @on-ok="reloadTable" />
        <ElementAddEdit ref="elementEdit" @on-ok="reloadTable" />
    </div>
</template>

<script>
export default {
    name: 'Module',
    components: {
        ModuleAddEdit: () => import('./moduleaddedit'), // 模块添加编辑
        ElementAddEdit: () => import('./elementaddedit') // 按钮添加编辑
    },
    data() {
        return {
            tableUrl: `${process.env.VUE_APP_API_ROOT}api/system/getmoduleelementlist`,
            // 过滤项
            filter: {
                moduleId: null
            },
            moduleTreeData: [],
            screenHeight: null,
            cardBodyHeight: null
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
        }
    },
    mounted() {
        const that = this
        that.loadModuleTree()
        window.addEventListener('resize', () => {
            //  resize:当调整浏览器窗口的大小时触发事件
            window.screenHeight = document.documentElement.clientHeight
            that.screenHeight = window.screenHeight - 73
        })
        that.screenHeight = document.documentElement.clientHeight - 73
        that.cardBodyHeight = that.screenHeight - 98
    },
    methods: {
        reloadTable(again) {
            this.$refs.table.reload(again)
        },
        onExpand() {
            this.$refs.table.resize()
        },
        // 获取模块表格树
        loadModuleTree() {
            this.$api.get('api/system/getmoduletree').then(res => {
                this.moduleTreeData = res.data
            })
        },
        // 模块选择事件
        radChange(id) {
            this.filter.moduleId = id
            this.reloadTable(true)
        },
        moduleAdd() {
            this.$refs.moduleEdit.open()
        },
        moduleEdit() {
            if (this.filter.moduleId == null) {
                return this.$message.warning('请选择要编辑的数据')
            }
            this.$refs.moduleEdit.open(this.filter.moduleId)
        },
        moduleRemove() {},
        elementAdd() {
            this.$refs.elementEdit.open()
        }
    }
}
</script>
