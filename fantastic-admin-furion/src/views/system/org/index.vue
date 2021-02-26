<template>
    <div class="page-main">
        <el-row :gutter="6">
            <el-col :span="4">
                <OrgTree :is-root="false" @tree-node-click="treeNodeClick" />
            </el-col>
            <el-col :span="20">
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
                                <el-button type="success" icon="el-icon-plus" @click="add">
                                    添加
                                </el-button>
                            </el-button-group>
                        </el-form-item>
                    </template>
                    <!-- 默认搜索项 -->
                    <el-form-item>
                        <el-input
                            v-model="filter.orgName"
                            placeholder="部门"
                            clearable
                        />
                    </el-form-item>
                    <template slot="more">
                        <!-- 更多搜索项 -->
                    </template>
                </SimpleHead>
                <!-- table -->
                <SimpleTable ref="table" :url="tableUrl" :filter="filter" :offset="70">
                    <el-table-column type="selection" width="40" fixed="left" />
                    <el-table-column
                        prop="orgId"
                        label="部门ID"
                        width="60"
                    />
                    <el-table-column prop="orgName" label="部门" />>
                    <el-table-column
                        prop="parentOrgName"
                        label="上级部门"
                    />>
                    <el-table-column prop="status" label="状态" width="120">
                        <template slot-scope="scope">
                            <el-tag
                                :type="scope.row.status === 1 ? 'success' : 'danger'"
                                disable-transitions
                            >
                                {{ scope.row.status === 1 ? "启用" : "禁用" }}
                            </el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column prop="operate" label="操作" width="80">
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
        <!-- 添加编辑 -->
        <AddEdit ref="edit" @on-ok="reloadTable" />
    </div>
</template>

<script>
export default {
    name: 'Org',
    components: {
        OrgTree: () => import('./orgtree'), // 部门树
        AddEdit: () => import('./addedit') // 添加编辑
    },
    data() {
        return {
            tableUrl: `${process.env.VUE_APP_API_ROOT}api/system/getorglist`,
            // 过滤项
            filter: {
                orgName: null,
                parentOrgId: null
            }
        }
    },
    methods: {
        reloadTable(again) {
            this.$refs.table.reload(again)
        },
        onExpand() {
            this.$refs.table.resize()
        },
        treeNodeClick(data) {
            this.filter.parentOrgId = data.id
            this.reloadTable(true)
        },
        add() {
            this.$refs.edit.open()
        },
        edit(row) {
            if (!row) {
                const rows = this.$refs.table.getSelection()
                if (rows.length !== 1) {
                    this.$message.warning('请选择单条数据进行操作.')
                    return
                }
                row = rows[0]
            }
            this.$refs.edit.open(row)
        },
        remove(row) {
            let ids = []
            if (!row) {
                const rows = this.$refs.table.getSelection()
                if (rows.length === 0) {
                    this.$message.warning('请选择要删除的部门')
                    return
                }
                ids = rows.map(e => e.userId)
            }

            this.$confirm('确定要删除部门信息吗？', '删除', {
                distinguishCancelAndClose: true,
                confirmButtonText: '确定',
                cancelButtonText: '取消'
            })
                .then(() => {
                    this.$api.post('api/system/orgremove', ids).then(res => {
                        if (res.success) {
                            this.$message.success(res.data)
                            this.reloadTable()
                        } else {
                            this.$message.error(res.msg)
                        }
                    })
                })
                .catch(error => {
                    console.log(error)
                })
        }
    }
}
</script>
