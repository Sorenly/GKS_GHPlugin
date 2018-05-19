using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GKS_Summary1802
{
    public class RebuildEdgeTest : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RebuildEdgeTest class.
        /// </summary>
        public RebuildEdgeTest()
            : base("重建边缘测试", "RebuildEdgeTest",
                "重建边缘测试",
                "GKS", "Surface")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("嵌面", "Path", "使用嵌面生成的结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("结果", "R", "Result of Rebuild Edge对嵌面使用边缘重建的结果", GH_ParamAccess.item);
            pManager.AddGenericParameter("结果2", "R", "Result of Rebuild Edge对嵌面使用边缘重建的结果", GH_ParamAccess.item);
            pManager.AddGenericParameter("结果3", "R", "Result of Rebuild Edge对嵌面使用边缘重建的结果", GH_ParamAccess.item);
            pManager.AddGenericParameter("结果4", "R", "Result of Rebuild Edge对嵌面使用边缘重建的结果", GH_ParamAccess.item);
            pManager.AddGenericParameter("输入边", "IE", "输入的嵌面的边", GH_ParamAccess.list);
            pManager.AddGenericParameter("输出边", "OE", "输出的Brep的边", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep InputBrep = null;
            if (DA.GetData(0,ref InputBrep) && InputBrep.IsValid)
            {
                var TempBrep = (Brep)InputBrep.DuplicateShallow();
                List<Curve> OriginEdge = new List<Curve>();
                foreach (var Cr in TempBrep.Edges)
                {
                    OriginEdge.Add(Cr.DuplicateCurve());
                }
                //DA.SetDataList(1, OriginEdge);
                
                TempBrep.Faces.ShrinkFaces();
                TempBrep.Faces.SplitKinkyFaces();
                TempBrep.Faces.StandardizeFaceSurfaces();
                TempBrep.Faces[0].RebuildEdges(0.001, false, false);
                DA.SetData(0, TempBrep.DuplicateBrep());
                if (!TempBrep.Faces[0].RebuildEdges(0.001, false, false))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "重建返回结果为失败");
                }
                DA.SetData(2, TempBrep.DuplicateBrep());
                
                List<Curve> OutPutCrList = new List<Curve>();
                foreach (var Cr in TempBrep.Edges)
                {
                    OutPutCrList.Add(Cr.DuplicateCurve());
                }
                DA.SetDataList(5, OutPutCrList);
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0735a3a8-9aae-4b3e-9bd2-f60f43ef5d27"); }
        }
    }
}