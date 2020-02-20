function torque = getTorque(force, forcePosition, forceAngle, centerOfMass)
%getTourqer Inputs: force, forcePosition, forceAngle, centerOfMass
%           Output: torque

r = pdist([forcePosition;centerOfMass],'euclidean');
r_angle = atan2((forcePosition(2)-centerOfMass(2)), ...
    (forcePosition(1)-centerOfMass(1)));
theta = forceAngle - r_angle;
torque = force*r*sin(theta);
end

