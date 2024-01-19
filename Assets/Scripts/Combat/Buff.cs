using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface Buff
{
    bool isExpired();
    void updateEffect(Unit unitToAffect);
    void removeEffect(Unit unitToAffect);

    void setDuration(float newDuration);

}