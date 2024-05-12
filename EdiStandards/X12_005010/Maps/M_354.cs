using EdiEngine.Common.Enums;
using EdiEngine.Common.Definitions;
using EdiEngine.Standards.X12_005010.Segments;

namespace EdiEngine.Standards.X12_005010.Maps
{
	public class M_354 : MapLoop
	{
		public M_354() : base(null)
		{
			Content.AddRange(new MapBaseEntity[] {
				new M10() { ReqDes = RequirementDesignator.Mandatory, MaxOccurs = 1 },
				new L_P4(this) { ReqDes = RequirementDesignator.Mandatory, MaxOccurs = 20 },
			});
		}

		//1000
		public class L_P4 : MapLoop
		{
			public L_P4(MapLoop parentLoop) : base(parentLoop)
			{
				Content.AddRange(new MapBaseEntity[] {
					new P4() { ReqDes = RequirementDesignator.Mandatory, MaxOccurs = 1 },
					new X01() { ReqDes = RequirementDesignator.Mandatory, MaxOccurs = 1 },
					new X02() { ReqDes = RequirementDesignator.Optional, MaxOccurs = 9999 },
				});
			}
		}

	}
 }

